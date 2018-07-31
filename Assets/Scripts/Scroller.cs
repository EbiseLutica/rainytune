using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Scroller : MonoBehaviour {

	float velocityX, velocityY;

	Vector3 dragStartPos;
	bool isMouseDragging;
	

	// Update is called once per frame
	void Update () {
		DoMouseWheelScroll();
		DoKeyScroll();
		DoMouseDragScroll();

		if (velocityX > 4) velocityX = 4;
		if (velocityX < -4) velocityX = -4;
		if (velocityY > 4) velocityY = 4;
		if (velocityY < -4) velocityY = -4;

		velocityX *= 0.8f;
		velocityY *= 0.8f;

		transform.Translate(new Vector3(velocityX, 0, velocityY));

		if (transform.position.x < -42)
		{
			transform.position = new Vector3(-42, transform.position.y, transform.position.z);
		}


		if (transform.position.x > 53)
		{
			transform.position = new Vector3(53, transform.position.y, transform.position.z);
		}

		if (transform.position.z < -32)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, -32);
		}


		if (transform.position.z > -15)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, -15);
		}
	}

	void DoMouseWheelScroll()
	{
		var d = Input.mouseScrollDelta;
		velocityX += d.x;
		velocityY += d.y;

	}

	void DoKeyScroll()
	{
		if (Input.GetKey(KeyCode.LeftArrow)) velocityX -= 0.4f;
		if (Input.GetKey(KeyCode.RightArrow)) velocityX += 0.4f;
		if (Input.GetKey(KeyCode.UpArrow)) velocityY += 0.4f;
		if (Input.GetKey(KeyCode.DownArrow)) velocityY -= 0.4f;

	}

	void DoMouseDragScroll()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragStartPos = Input.mousePosition;
			isMouseDragging = true;
		}

		if (Input.GetMouseButtonUp(0))
		{
			isMouseDragging = false;
		}

		if (isMouseDragging)
		{
			velocityX += (dragStartPos.x - Input.mousePosition.x) / 32f;
			velocityY += (Input.mousePosition.y - dragStartPos.y) / 32f;

			dragStartPos = Input.mousePosition;
		}

	}



}
