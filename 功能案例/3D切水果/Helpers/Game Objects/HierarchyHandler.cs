using UnityEngine;

[RequireComponent(typeof(ShatterTool))]
public class HierarchyHandler : MonoBehaviour
{
	public bool attachPieceToParent = true;
	public float maxPieceToParentDistance = 1.0f;
	public bool addRbToDetachedPieces = true;
	
	public bool attachChildrenToPieces = true;
	public float maxChildToPieceDistance = 1.0f;
	public bool addRbToDetachedChildren = true;
	
	private Transform parent;
	private Transform[] children;
	
	public void PreSplit(Plane[] planes)
	{
		// Save and detach parent
		if (transform.parent != null)
		{
			parent = transform.parent;
			
			transform.parent = null;
		}
		
		// Save and detach children
		children = new Transform[transform.childCount];
		
		int index = 0;
		
		foreach (Transform child in transform)
		{
			children[index++] = child;
		}
		
		transform.DetachChildren();
	}
	
	public void PostSplit(GameObject[] newGameObjects)
	{
		// Get shatter tool scripts of the new game objects
		ShatterTool[] pieces = new ShatterTool[newGameObjects.Length];
		
		for (int i = 0; i < newGameObjects.Length; i++)
		{
			pieces[i] = newGameObjects[i].GetComponent<ShatterTool>();
		}
		
		// Attach one of the new pieces to the original parent
		if (parent != null)
		{
			ShatterTool parentShatterTool = parent.GetComponent<ShatterTool>();
			
			if (parentShatterTool != null)
			{
				// Which piece should attach to the original parent?
				ShatterTool closestPiece = null;
				
				if (attachPieceToParent)
				{
					closestPiece = FindClosestPiece(parentShatterTool, pieces, maxPieceToParentDistance);
					
					if (closestPiece != null)
					{
						closestPiece.transform.parent = parent;
					}
				}
				
				// Add rigidbodies to the detached pieces
				if (addRbToDetachedPieces)
				{
					foreach (ShatterTool piece in pieces)
					{
						if (piece != null && piece != closestPiece)
						{
							piece.gameObject.AddComponent<Rigidbody>();
						}
					}
				}
			}
		}
		
		// Attach the original children to the new pieces
		foreach (Transform child in children)
		{
			ShatterTool childShatterTool = child.GetComponent<ShatterTool>();
			
			if (childShatterTool != null)
			{
				// Which piece should this child attach to?
				ShatterTool closestPiece = FindClosestPiece(childShatterTool, pieces, maxChildToPieceDistance);
				
				if (attachChildrenToPieces && closestPiece != null)
				{
					child.parent = closestPiece.transform;
				}
				else
				{
					if (addRbToDetachedChildren)
					{
						child.gameObject.AddComponent<Rigidbody>();
					}
				}
			}
		}
	}
	
	private ShatterTool FindClosestPiece(ShatterTool reference, ShatterTool[] pieces, float maxDistance)
	{
		Vector3 center = reference.Center;
		float maxDistanceSqr = maxDistance * maxDistance;
		
		ShatterTool closestPiece = null;
		float closestDistanceSqr = 0.0f;
		
		for (int i = 0; i < pieces.Length; i++)
		{
			ShatterTool piece = pieces[i];
			
			if (piece != null)
			{
				float distanceSqr = (center - piece.Center).sqrMagnitude;
				
				if (distanceSqr < maxDistanceSqr && (distanceSqr < closestDistanceSqr || closestPiece == null))
				{
					closestPiece = piece;
					closestDistanceSqr = distanceSqr;
				}
			}
		}
		
		return closestPiece;
	}
}