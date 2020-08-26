using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Xenon {
	public struct Transform2D {

		/// <summary>
		/// The underlying unity transform
		/// </summary>
		public Transform transform { get; private set; }

		/// <summary>
		/// The parent of the transform.
		/// </summary>
		public Transform parent {
			get { return transform.parent; }
			set { transform.parent = value; }
		}

		/// <summary>
		/// Returns the topmost transform in the hierarchy.
		/// </summary>
		public Transform root {
			get { return transform.root; }
		}

		/// <summary>
		/// The position of the transform in world space.
		/// </summary>
		public Vector2 position {
			get { return transform.Position2D(); }
			set { transform.SetPosition2D(value); }
		}

		/// <summary>
		/// Position of the transform relative to the parent transform.
		/// </summary>
		public Vector2 localPosition {
			get { return transform.LocalPosition2D(); }
			set { transform.SetLocalPosition2D(value); }
		}

		/// <summary>
		/// The depth (z axis) of the transform in world space.
		/// </summary>
		public float depth {
			get { return transform.position.z; }
			set { transform.position = new Vector3(transform.position.x, transform.position.y, value); }
		}

		/// <summary>
		/// The depth (z axis) of the transform relative to the parent transform.
		/// </summary>
		public float localDepth {
			get { return transform.localPosition.z; }
			set { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value); }
		}

		/// <summary>
		/// The rotation of the transform in world space (in degrees).
		/// </summary>
		public float rotation {
			get { return transform.Rotation2D(); }
			set { transform.SetRotation2D(value); }
		}

		/// <summary>
		/// The rotation of the transform relative to the parent transform (in degrees).
		/// </summary>
		public float localRotation {
			get { return transform.LocalRotation2D(); }
			set { transform.SetLocalRotation2D(value); }
		}

		/// <summary>
		/// The global scale of the object (RO).
		/// </summary>
		public Vector2 lossyScale {
			get { return transform.Scale2D(); }
		}

		/// <summary>
		/// The scale of the transform relative to the parent.
		/// </summary>
		public Vector2 localScale {
			get { return transform.LocalScale2D(); }
			set { transform.SetLocalScale2D(value); }
		}

		/// <summary>
		/// The number of children the Transform has.
		/// </summary>
		public int childCount {
			get { return transform.childCount; }
		}

		/// <summary>
		/// The green axis of the transform in world space.
		/// </summary>
		public Vector2 up {
			get { return transform.up.ToVec2(); }
		}

		/// <summary>
		/// The red axis of the transform in world space.
		/// </summary>
		public Vector2 right {
			get { return transform.right.ToVec2(); }
		}

		/// <summary>
		/// Has the transform changed since the last time the flag was set to 'false'?
		/// </summary>
		public bool hasChanged {
			get { return transform.hasChanged; }
			set { transform.hasChanged = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int hierarchyCapacity {
			get { return transform.hierarchyCapacity; }
			set { transform.hierarchyCapacity = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int hierarchyCount {
			get { return transform.hierarchyCount; }
		}


		/// <summary>
		/// Creates a Transform2D wrapped around the specified unity Transform
		/// </summary>
		/// <param name="transform">the transform to wrap around</param>
		public Transform2D(Transform transform) {
			this.transform = transform;
		}

		/// <summary>
		/// Set the parent of the transform.
		/// </summary>
		/// <param name="p">The parent transform to use.</param>
		public void SetParent(Transform p) {
			transform.SetParent(p);
		}

		/// <summary>
		/// Set the parent of the transform.
		/// </summary>
		/// <param name="p">The parent Transform to use.</param>
		/// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
		public void SetParent(Transform p, bool worldPositionStays) {
			transform.SetParent(p, worldPositionStays);
		}

		/// <summary>
		/// Set the parent of the transform.
		/// </summary>
		/// <param name="p">The parent transform to use.</param>
		public void SetParent(Transform2D p) {
			SetParent(p.transform);
		}

		/// <summary>
		/// Set the parent of the transform.
		/// </summary>
		/// <param name="p">The parent Transform to use.</param>
		/// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
		public void SetParent(Transform2D p, bool worldPositionStays) {
			SetParent(p.transform, worldPositionStays);
		}

		/// <summary>
		/// Set position and rotation in world space
		/// </summary>
		public void SetPositionAndRotation(Vector2 position, float rotation) {
			transform.SetPositionAndRotation(position.ToVec3(transform.position.z), Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation));
		}

		/// <summary>
		/// Moves the transform in the direction and distance of /translation/.
		/// </summary>
		public void Translate(Vector2 translation, Space space = Space.Self) {
			transform.Translate(translation.ToVec3(), space);
		}

		/// <summary>
		/// Moves the transform by x along the x axis and y along the y axis.
		/// </summary>
		public void Translate(float x, float y, Space space = Space.Self) {
			transform.Translate(x, y, 0f, space);
		}

		/// <summary>
		/// Moves the transform in the direction and distance of /translation/.
		/// </summary>
		public void Translate(Vector2 translation, Transform relativeTo) {
			transform.Translate(translation.ToVec3(), relativeTo);
		}

		/// <summary>
		/// Moves the transform by x along the x axis and y along the y axis.
		/// </summary>
		public void Translate(float x, float y, Transform relativeTo) {
			transform.Translate(x, y, 0f, relativeTo);
		}

		/// <summary>
		/// Moves the transform in the direction and distance of /translation/.
		/// </summary>
		public void Translate(Vector2 translation, Transform2D relativeTo) {
			transform.Translate(translation.ToVec3(), relativeTo.transform);
		}

		/// <summary>
		/// Moves the transform by x along the x axis and y along the y axis.
		/// </summary>
		public void Translate(float x, float y, Transform2D relativeTo) {
			transform.Translate(x, y, 0f, relativeTo.transform);
		}

		/// <summary>
		/// Applies a rotation of `angle` degrees around the z-axis.
		/// </summary>
		/// <param name="angle">The rotation to apply.</param>
		/// <param name="space">Determines whether to rotate the GameObject either locally to the GameObject
		/// or relative to the Scene in world space.</param>
		public void Rotate(float angle, Space space = Space.Self) {
			transform.Rotate(new Vector3(0f, 0f, angle), space);
		}

		/// <summary>
		/// Rotates the transform around an point in world space.
		/// </summary>
		public void RotateAround(Vector2 point, float angle) {
			Vector2 dif = position - point;
			dif = Quaternion.Euler(0f, 0f, angle) * dif;
			position = point + dif;
			Rotate(angle);
		}

		/// <summary>
		/// Rotates the transform so the localDir points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		/// <param name="localDir">The local direction that needs to be pointed towards the target.</param>
		public void LookAt(Transform target, Vector2 localDir) {
			LookAt(target.position, localDir);
		}

		/// <summary>
		/// Rotates the transform so the up vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtUp(Transform target) {
			LookAt(target, Vector2.up);
		}

		/// <summary>
		/// Rotates the transform so the down vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtDown(Transform target) {
			LookAt(target, Vector2.down);
		}

		/// <summary>
		/// Rotates the transform so the left vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtLeft(Transform target) {
			LookAt(target, Vector2.right);
		}

		/// <summary>
		/// Rotates the transform so the right vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtRight(Transform target) {
			LookAt(target, Vector2.right);
		}

		/// <summary>
		/// Rotates the transform so the localDir points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		/// <param name="localDir">The local direction that needs to be pointed towards the target.</param>
		public void LookAt(Transform2D target, Vector2 localDir) {
			LookAt(target.transform, localDir);
		}

		/// <summary>
		/// Rotates the transform so the up vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtUp(Transform2D target) {
			LookAt(target.transform, Vector2.up);
		}

		/// <summary>
		/// Rotates the transform so the down vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtDown(Transform2D target) {
			LookAt(target.transform, Vector2.down);
		}

		/// <summary>
		/// Rotates the transform so the left vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtLeft(Transform2D target) {
			LookAt(target.transform, Vector2.right);
		}

		/// <summary>
		/// Rotates the transform so the right vector points at target's current position.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtRight(Transform2D target) {
			LookAt(target.transform, Vector2.right);
		}

		/// <summary>
		/// Rotates the transform so the localDir points at target.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		/// <param name="localDir">The local direction that needs to be pointed towards the target.</param>
		public void LookAt(Vector2 target, Vector2 localDir) {
			rotation = Vector2.SignedAngle(TransformDirection(localDir), target - position);
		}

		/// <summary>
		/// Rotates the transform so the up vector points at target.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtUp(Vector2 target) {
			LookAt(target, Vector2.up);
		}

		/// <summary>
		/// Rotates the transform so the down vector points at target.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtDown(Vector2 target) {
			LookAt(target, Vector2.down);
		}

		/// <summary>
		/// Rotates the transform so the left vector points at target.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtLeft(Vector2 target) {
			LookAt(target, Vector2.left);
		}

		/// <summary>
		/// Rotates the transform so the right vector points at target.
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		public void LookAtRight(Vector2 target) {
			LookAt(target, Vector2.right);
		}

		/// <summary>
		/// Transforms direction from local space to world space.
		/// </summary>
		public Vector2 TransformDirection(Vector2 direction) {
			return transform.TransformDirection(direction.ToVec3()).ToVec2();
		}

		/// <summary>
		/// Transforms direction x, y from local space to world space.
		/// </summary>
		public Vector2 TransformDirection(float x, float y) {
			return TransformDirection(new Vector2(x, y));
		}

		/// <summary>
		/// Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.
		/// </summary>
		public Vector2 InverseTransformDirection(Vector2 direction) {
			return transform.InverseTransformDirection(direction.ToVec3()).ToVec2();
		}

		/// <summary>
		/// Transforms the direction x, y from world space to local space. The opposite of Transform.TransformDirection.
		/// </summary>
		public Vector2 InverseTransformDirection(float x, float y) {
			return InverseTransformDirection(new Vector2(x, y));
		}

		/// <summary>
		/// Transforms vector from local space to world space.
		/// </summary>
		public Vector2 TransformVector(Vector2 vector) {
			return transform.TransformVector(vector.ToVec3());
		}

		/// <summary>
		/// Transforms vector x, y from local space to world space.
		/// </summary>
		public Vector2 TransformVector(float x, float y) {
			return TransformVector(new Vector2(x, y));
		}

		/// <summary>
		/// Transforms a vector from world space to local space. The opposite of Transform.TransformVector.
		/// </summary>
		public Vector2 InverseTransformVector(Vector2 vector) {
			return transform.InverseTransformVector(vector.ToVec3()).ToVec2();
		}

		/// <summary>
		/// Transforms the vector x, y from world space to local space. The opposite of
		/// Transform.TransformVector.
		/// </summary>
		public Vector2 InverseTransformVector(float x, float y) {
			return InverseTransformVector(new Vector2(x, y));
		}

		/// <summary>
		/// Transforms position from local space to world space.
		/// </summary>
		public Vector2 TransformPoint(Vector2 point) {
			return transform.TransformPoint(point.ToVec3()).ToVec2();
		}

		/// <summary>
		/// Transforms the position x, y from local space to world space.
		/// </summary>
		public Vector2 TransformPoint(float x, float y) {
			return TransformPoint(new Vector2(x, y));
		}

		/// <summary>
		/// Transforms position from world space to local space.
		/// </summary>
		public Vector2 InverseTransformPoint(Vector2 point) {
			return transform.InverseTransformPoint(point.ToVec3()).ToVec2();
		}

		/// <summary>
		/// Transforms the position x, y from world space to local space. The opposite of Transform.TransformPoint.
		/// </summary>
		public Vector2 InverseTransformPoint(float x, float y) {
			return InverseTransformPoint(new Vector2(x, y));
		}

		/// <summary>
		/// Unparents all children.
		/// </summary>
		public void DetachChildren() {
			transform.DetachChildren();
		}

		/// <summary>
		/// Move the transform to the start of the local transform list.
		/// </summary>
		public void SetAsFirstSibling() {
			transform.SetAsLastSibling();
		}

		/// <summary>
		/// Move the transform to the end of the local transform list.
		/// </summary>
		public void SetAsLastSibling() {
			transform.SetAsLastSibling();
		}

		/// <summary>
		/// Sets the sibling index.
		/// </summary>
		/// <param name="index">Index to set.</param>
		public void SetSiblingIndex(int index) {
			transform.SetSiblingIndex(index);
		}

		/// <summary>
		/// Gets the sibling index.
		/// </summary>
		public int GetSiblingIndex() {
			return transform.GetSiblingIndex();
		}

		/// <summary>
		/// Finds a child by n and returns it.
		/// </summary>
		/// <param name="n">Name of child to be found.</param>
		/// <returns>The returned child transform or null if no child is found.</returns>
		public Transform2D Find(string n) {
			return new Transform2D(transform.Find(n));
		}

		/// <summary>
		/// Is this transform a child of parent?
		/// </summary>
		public bool IsChildOf(Transform parent) {
			return transform.IsChildOf(parent);
		}

		/// <summary>
		/// Is this transform a child of parent?
		/// </summary>
		public bool IsChildOf(Transform2D parent) {
			return transform.IsChildOf(parent.transform);
		}

		public IEnumerator GetEnumerator() {
			return transform.GetEnumerator();
		}
		/// <summary>
		/// Returns a transform child by index.
		/// </summary>
		/// <param name="index">Index of the child transform to return. Must be smaller than Transform.childCount.</param>
		/// <returns>Transform child by index.</returns>
		public Transform2D GetChild(int index) {
			return new Transform2D(transform.GetChild(index));
		}

		public override bool Equals(object obj) {
			return obj is Transform2D d &&
				   EqualityComparer<Transform>.Default.Equals(transform, d.transform);
		}

		public override int GetHashCode() {
			return transform.GetHashCode();
		}

		public static bool operator==(Transform2D a, Transform2D b) {
			return a.transform == b.transform;
		}

		public static bool operator!=(Transform2D a, Transform2D b) {
			return a.transform != b.transform;
		}

	}
}
