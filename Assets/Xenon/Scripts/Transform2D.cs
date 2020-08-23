using System.Collections;

using UnityEngine;

namespace Xenon {
	public struct Transform2D {

		public Transform transform { get; private set; }

		public Transform parent {
			get { return transform.parent; }
			set { transform.parent = value; }
		}

		public Transform root {
			get { return transform.root; }
		}

		public Vector2 position {
			get { return transform.Position2D(); }
			set { transform.SetPosition2D(value); }
		}

		public Vector2 localPosition {
			get { return transform.LocalPosition2D(); }
			set { transform.SetLocalPosition2D(value); }
		}

		public float depth {
			get { return transform.position.z; }
			set { transform.position = new Vector3(transform.position.x, transform.position.y, value); }
		}

		public float localDepth {
			get { return transform.localPosition.z; }
			set { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value); }
		}

		public float rotation {
			get { return transform.Rotation2D(); }
			set { transform.SetRotation2D(value); }
		}

		public float localRotation {
			get { return transform.LocalRotation2D(); }
			set { transform.SetLocalRotation2D(value); }
		}

		public Vector2 lossyScale {
			get { return transform.Scale2D(); }
		}

		public Vector2 localScale {
			get { return transform.LocalScale2D(); }
			set { transform.SetLocalScale2D(value); }
		}

		public int childCount {
			get { return transform.childCount; }
		}

		public Vector2 up {
			get { return transform.up.ToVec2(); }
		}

		public Vector2 right {
			get { return transform.right.ToVec2(); }
		}

		public bool hasChanged {
			get { return transform.hasChanged; }
			set { transform.hasChanged = value; }
		}

		public int hierarchyCapacity {
			get { return transform.hierarchyCapacity; }
			set { transform.hierarchyCapacity = value; }
		}

		public int hierarchyCount {
			get { return transform.hierarchyCount; }
		}


		public Transform2D(Transform transform) {
			this.transform = transform;
		}

		public void SetParent(Transform p) {
			transform.SetParent(p);
		}

		public void SetParent(Transform p, bool worldPositionStays) {
			transform.SetParent(p, worldPositionStays);
		}

		public void SetPositionAndRotation(Vector2 position, float rotation) {
			transform.SetPositionAndRotation(position.ToVec3(transform.position.z), Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation));
		}

		public void Translate(Vector2 translation, Space space = Space.Self) {
			transform.Translate(translation.ToVec3(), space);
		}

		public void Translate(float x, float y, Space space = Space.Self) {
			transform.Translate(x, y, 0f, space);
		}

		public void Translate(Vector2 translation, Transform relativeTo) {
			transform.Translate(translation.ToVec3(), relativeTo);
		}

		public void Translate(float x, float y, Transform relativeTo) {
			transform.Translate(x, y, 0f, relativeTo);
		}

		public void Rotate(float angle, Space space = Space.Self) {
			transform.Rotate(new Vector3(0f, 0f, angle), space);
		}

		public void RotateAround(Vector2 point, float angle) {
			Vector2 dif = position - point;
			dif = Quaternion.Euler(0f, 0f, angle) * dif;
			position = point + dif;
			Rotate(angle);
		}

		public void LookAt(Transform target, Vector2 localDir) {
			LookAt(target.position, localDir);
		}

		public void LookAtUp(Transform target) {
			LookAt(target, Vector2.up);
		}

		public void LookAtRight(Transform target) {
			LookAt(target, Vector2.right);
		}

		public void LookAt(Vector2 target, Vector2 localDir) {
			rotation = Vector2.SignedAngle(TransformDirection(localDir), target - position);
		}

		public void LookAtUp(Vector2 target) {
			LookAt(target, Vector2.up);
		}

		public void LookAtRight(Vector2 target) {
			LookAt(target, Vector2.right);
		}

		public Vector2 TransformDirection(Vector2 direction) {
			return transform.TransformDirection(direction.ToVec3()).ToVec2();
		}

		public Vector2 TransformDirection(float x, float y) {
			return TransformDirection(new Vector2(x, y));
		}

		public Vector2 InverseTransformDirection(Vector2 direction) {
			return transform.InverseTransformDirection(direction.ToVec3()).ToVec2();
		}

		public Vector2 InverseTransformDirection(float x, float y) {
			return InverseTransformDirection(new Vector2(x, y));
		}

		public Vector2 TransformVector(Vector2 vector) {
			return transform.TransformVector(vector.ToVec3());
		}

		public Vector2 TransformVector(float x, float y) {
			return TransformVector(new Vector2(x, y));
		}

		public Vector2 InverseTransformVector(Vector2 vector) {
			return transform.InverseTransformVector(vector.ToVec3()).ToVec2();
		}

		public Vector2 InverseTransformVector(float x, float y) {
			return InverseTransformVector(new Vector2(x, y));
		}

		public Vector2 TransformPoint(Vector2 point) {
			return transform.TransformPoint(point.ToVec3()).ToVec2();
		}

		public Vector2 TransformPoint(float x, float y) {
			return TransformPoint(new Vector2(x, y));
		}

		public Vector2 InverseTransformPoint(Vector2 point) {
			return transform.InverseTransformPoint(point.ToVec3()).ToVec2();
		}

		public Vector2 InverseTransformPoint(float x, float y) {
			return InverseTransformPoint(new Vector2(x, y));
		}

		public void DetachChildren() {
			transform.DetachChildren();
		}

		public void SetAsFirstSibling() {
			transform.SetAsLastSibling();
		}

		public void SetAsLastSibling() {
			transform.SetAsLastSibling();
		}

		public void SetSiblingIndex(int index) {
			transform.SetSiblingIndex(index);
		}

		public int GetSiblingIndex() {
			return transform.GetSiblingIndex();
		}

		public Transform Find(string n) {
			return transform.Find(n);
		}

		public bool IsChildOf(Transform parent) {
			return transform.IsChildOf(parent);
		}

		public IEnumerator GetEnumerator() {
			return transform.GetEnumerator();
		}

		public Transform GetChild(int index) {
			return transform.GetChild(index);
		}

	}
}
