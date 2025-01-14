﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    public enum Action { Null, LightAttack, HeavyAttack, Jump, Grab, Throw, PickupOrGrab }

    public float moveRadius = 50;
    public static float _horizontalAxisValue = 0;
    public static float _verticalAxisValue = 0;
    public static Action _lastAction = Action.Null;

    public Touch nullTouch;
    public Touch startMoveTouch;
    public Touch startActionTouch;
    public bool touchHold;
    public float touchHoldTimer;
    public float touchHoldTime = 0.7f;
    private float rightClickTimer;

    private void Start()
    {
        nullTouch = new Touch();
        startMoveTouch = nullTouch;
        startActionTouch = nullTouch;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (isMoveTouch(touch))
                {
                    if (touch.phase == TouchPhase.Began)
                        SetMoveTouch(touch);
                    else if (touch.phase == TouchPhase.Moved)
                        SetMoveAxis(touch);
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        startMoveTouch = nullTouch;
                        _horizontalAxisValue = 0;
                        _verticalAxisValue = 0;
                    }
                }
                else if (isActionTouch(touch))
                {
                    if (touch.phase == TouchPhase.Began)
                        SetActionTouch(touch);
                    else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                        CheckHold(touch);
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                        SetAction(touch);
                }
            }
        }
        else
        {
            _horizontalAxisValue = Input.GetAxisRaw("Horizontal");
            _verticalAxisValue = Input.GetAxisRaw("Vertical");

            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl))
            {
                touchHoldTimer = 0;
            }
            else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftControl))
            {
                touchHoldTimer += Time.deltaTime;
                if (touchHoldTimer >= touchHoldTime)
                {
                    _lastAction = Action.PickupOrGrab;
                    touchHoldTimer = 0;
                    return;
                }
            }
            if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.LeftAlt))
            {
                rightClickTimer = 0;
            }
            else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt))
            {
                rightClickTimer += Time.deltaTime;
                if (rightClickTimer >= touchHoldTime)
                {
                    _lastAction = Action.Throw;
                    touchHoldTimer = 0;
                    return;
                }
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
                _lastAction = Action.LightAttack;
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftAlt))
                _lastAction = Action.HeavyAttack;
            else if (Input.GetKeyDown(KeyCode.Space))
                _lastAction = Action.Jump;
            else if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
                _lastAction = Action.PickupOrGrab;
            else if (Input.GetKeyDown(KeyCode.E))
                _lastAction = Action.Throw;
            else
                _lastAction = Action.Null;
        }
    }

    private void CheckHold(Touch actionTouch)
    {
        if (!touchHold) return;

        touchHoldTimer += Time.deltaTime;

        Vector2 deltaPosition = actionTouch.position - startActionTouch.position;
        if (deltaPosition.magnitude > 10)
            touchHold = false;

        if (touchHold && touchHoldTimer >= touchHoldTime)
        {
            touchHold = false;
            touchHoldTimer = 0;
            startActionTouch = nullTouch;
            _lastAction = Action.PickupOrGrab;
        }
    }

    private bool isActionTouch(Touch touch)
    {
        if (!startActionTouch.Equals(nullTouch))
            if (startActionTouch.fingerId == touch.fingerId)
                return true;

        if (touch.phase == TouchPhase.Began)
            if (touch.position.x > Screen.width / 2)
                return true;

        return false;
    }

    public static Action GetAction()
    {
        Action action = _lastAction;
        _lastAction = Action.Null;
        return action;
    }

    private void SetAction(Touch actionTouch)
    {
        Vector2 deltaPosition = actionTouch.position - startActionTouch.position;
        if (touchHold && touchHoldTimer >= touchHoldTime)
            _lastAction = Action.PickupOrGrab;
        else if (deltaPosition.magnitude < 10)
            _lastAction = Action.LightAttack;
        else if (SwipeLeft(deltaPosition))
            _lastAction = Action.Throw;
        else if (SwipeRight(deltaPosition))
            _lastAction = Action.HeavyAttack;
        else if (SwipeUp(deltaPosition))
            _lastAction = Action.Jump;
        else // if (SwipeDown(deltaPosition))
            _lastAction = Action.PickupOrGrab;

        startActionTouch = nullTouch;
        touchHold = false;
        touchHoldTimer = 0;
    }

    private bool SwipeLeft(Vector2 deltaPosition)
    {
        if (Math.Abs(deltaPosition.x) >= Math.Abs(deltaPosition.y))
            if (deltaPosition.x < 0)
                return true;

        return false;
    }

    private bool SwipeRight(Vector2 deltaPosition)
    {
        if (Math.Abs(deltaPosition.x) >= Math.Abs(deltaPosition.y))
            if (deltaPosition.x > 0)
                return true;

        return false;
    }

    private bool SwipeUp(Vector2 deltaPosition)
    {
        if (Math.Abs(deltaPosition.y) >= Math.Abs(deltaPosition.x))
            if (deltaPosition.y > 0)
                return true;

        return false;
    }

    private void SetMoveAxis(Touch moveTouch)
    {
        _horizontalAxisValue = moveTouch.position.x;
        _horizontalAxisValue -= startMoveTouch.position.x;
        _horizontalAxisValue /= moveRadius;
        _horizontalAxisValue = Mathf.Clamp(_horizontalAxisValue, -1, 1);

        _verticalAxisValue = moveTouch.position.y;
        _verticalAxisValue -= startMoveTouch.position.y;
        _verticalAxisValue /= moveRadius;
        _verticalAxisValue = Mathf.Clamp(_verticalAxisValue, -1, 1);
    }

    private void SetActionTouch(Touch touch)
    {
        if (!startActionTouch.Equals(nullTouch))
            return;

        startActionTouch = touch;
        touchHold = true;
    }

    private void SetMoveTouch(Touch touch)
    {
        if (!startMoveTouch.Equals(nullTouch))
            return;

        startMoveTouch = touch;
    }

    private bool isMoveTouch(Touch touch)
    {
        if (!startMoveTouch.Equals(nullTouch))
            if (startMoveTouch.fingerId == touch.fingerId)
                return true;

        if (touch.phase == TouchPhase.Began)
            if (touch.position.x <= Screen.width / 2)
                return true;

        return false;
    }

    public static float GetAxis(string axisName)
    {
        if (axisName == "Horizontal")
            return _horizontalAxisValue;
        if (axisName == "Vertical")
            return _verticalAxisValue;
        else
            return -1;
    }
}
