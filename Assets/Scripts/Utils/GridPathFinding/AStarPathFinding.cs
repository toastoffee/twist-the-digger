using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWalkable<T> {
  bool isWalkable();
}

public static class AStarPathFinding {
  public class GridNode {
    public Vector2Int xy; // idx
    public int x => xy.x;
    public int y => xy.y;
    public bool walkable;
    public int gCost; // from start to node (Manhattan)
    public int hCost; // from node to end (Manhattan)
    public GridNode parent;

    public int fCost => gCost + hCost;

    public GridNode(int x, int y, bool walkable) {
      xy = new Vector2Int(x, y);
      this.walkable = walkable;
    }

    public GridNode(Vector2Int xy, bool walkable) {
      this.xy = xy;
      this.walkable = walkable;
    }

    public override bool Equals(object obj) {
      if (obj == null) return false;

      if (GetType() != obj.GetType()) return false;

      GridNode other = (GridNode)obj;
      return xy.Equals(other.xy);
    }

    public static bool operator==(GridNode left, GridNode right) {
      if (left is null) return right is null;
      return left.Equals(right);
    }

    public static bool operator!=(GridNode left, GridNode right) {
      return !(left == right);
    }

    public override int GetHashCode() {
      return xy.GetHashCode();
    }
  }

  private static readonly Vector2Int[] directions = new Vector2Int[] {
    Vector2Int.up,
    Vector2Int.down,
    Vector2Int.left,
    Vector2Int.right,
  };

  public static List<Vector2Int> FindPath<T>(Grid<T> grid, Predicate<T> isWalkable, Vector2Int start, Vector2Int end) {
    GridNode[,] gridNodes = new GridNode[grid.size.x, grid.size.y];

    // check if start and end (index out of range)
    if (!grid.IsPositionValid(start) || !grid.IsPositionValid(end)) return null;

    // check if start and end (unwalkable)
    if (!isWalkable(grid[start]) || !isWalkable(grid[end])) return null;


    List<GridNode> openList = new List<GridNode>();
    HashSet<GridNode> closedSet = new HashSet<GridNode>();

    GridNode startGridNode = GetGridNode(grid, isWalkable, gridNodes, start);
    GridNode endGridNode = GetGridNode(grid, isWalkable, gridNodes, end);

    openList.Add(startGridNode);

    while (openList.Count > 0) {
      // get the lowest-fCost gridNode
      var lowestFCostNode = FindLowestFCostNode(openList);
      openList.Remove(lowestFCostNode);
      closedSet.Add(lowestFCostNode);

      if (lowestFCostNode == endGridNode) {
        return RetracePath(startGridNode, endGridNode);
      }

      var currentNode = lowestFCostNode;
      // check up-down-left-right
      foreach (var direction in directions) {
        var neighbourXY = currentNode.xy + direction;

        // check if neighbour is valid
        if (!grid.IsPositionValid(neighbourXY)) continue;

        GridNode neighbour = GetGridNode(grid, isWalkable, gridNodes, neighbourXY);

        // skip unwalkable or nodes already in closed set
        if (!neighbour.walkable || closedSet.Contains(neighbour))
          continue;

        // calc new gCost (move cost == 1)
        int newGCostToNeighbour = currentNode.gCost + 1;

        // if the new path is better or the neighbour is not in open set
        if (newGCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour)) {
          neighbour.gCost = newGCostToNeighbour;
          neighbour.hCost = ManhattanDistance(neighbour, endGridNode);
          neighbour.parent = currentNode;

          if (!openList.Contains(neighbour)) {
            openList.Add(neighbour);
          }
        }
      }
    }

    return null;
  }

  public static List<Vector2Int> FindPath<T>(Grid<T> grid, Vector2Int start, Vector2Int end)
    where T : IWalkable<T> {
    GridNode[,] gridNodes = new GridNode[grid.size.x, grid.size.y];

    // check if start and end (index out of range)
    if (!grid.IsPositionValid(start) || !grid.IsPositionValid(end)) return null;

    // check if start and end (unwalkable)
    if (!GetGridNode(grid, gridNodes, start).walkable || !GetGridNode(grid, gridNodes, end).walkable) return null;

    List<GridNode> openList = new List<GridNode>();
    HashSet<GridNode> closedSet = new HashSet<GridNode>();

    GridNode startGridNode = GetGridNode(grid, gridNodes, start);
    GridNode endGridNode = GetGridNode(grid, gridNodes, end);

    openList.Add(startGridNode);

    while (openList.Count > 0) {
      // get the lowest-fCost gridNode
      var lowestFCostNode = FindLowestFCostNode(openList);
      openList.Remove(lowestFCostNode);
      closedSet.Add(lowestFCostNode);

      if (lowestFCostNode == endGridNode) {
        return RetracePath(startGridNode, endGridNode);
      }

      var currentNode = lowestFCostNode;
      // check up-down-left-right
      foreach (var direction in directions) {
        var neighbourXY = currentNode.xy + direction;

        // check if neighbour is valid
        if (!grid.IsPositionValid(neighbourXY)) continue;

        GridNode neighbour = GetGridNode(grid, gridNodes, neighbourXY);

        // skip unwalkable or nodes already in closed set
        if (!neighbour.walkable || closedSet.Contains(neighbour))
          continue;

        // calc new gCost (move cost == 1)
        int newGCostToNeighbour = currentNode.gCost + 1;

        // if the new path is better or the neighbour is not in open set
        if (newGCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour)) {
          neighbour.gCost = newGCostToNeighbour;
          neighbour.hCost = ManhattanDistance(neighbour, endGridNode);
          neighbour.parent = currentNode;

          if (!openList.Contains(neighbour)) {
            openList.Add(neighbour);
          }
        }
      }
    }

    return null;
  }

  private static int ManhattanDistance(GridNode a, GridNode b) {
    return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
  }

  private static List<Vector2Int> RetracePath(GridNode startNode, GridNode endNode) {
    GridNode currentNode = endNode;

    List<Vector2Int> path = new List<Vector2Int>();
    while (currentNode != startNode) {
      path.Add(currentNode.xy);
      currentNode = currentNode.parent;
    }

    path.Reverse();
    return path;
  }

  private static GridNode FindLowestFCostNode(List<GridNode> nodes) {
    if (nodes.Count == 0) return null;

    var lowestFCostNode = nodes[0];
    for (int i = 1; i < nodes.Count; i++) {
      if (nodes[i].fCost < lowestFCostNode.fCost ||
          (nodes[i].fCost == lowestFCostNode.fCost && nodes[i].hCost < lowestFCostNode.hCost)) {
        lowestFCostNode = nodes[i];
      }
    }
    return lowestFCostNode;
  }

  private static GridNode GetGridNode<T>(Grid<T> grid, GridNode[,] gridNodes, Vector2Int xy)
    where T : IWalkable<T> {
    if (gridNodes[xy.x, xy.y] == null) {
      gridNodes[xy.x, xy.y] = new GridNode(xy.x, xy.y, grid[xy].isWalkable());
    }

    return gridNodes[xy.x, xy.y];
  }

  private static GridNode GetGridNode<T>(Grid<T> grid, Predicate<T> isWalkable, GridNode[,] gridNodes, Vector2Int xy) {
    if (gridNodes[xy.x, xy.y] == null) {
      gridNodes[xy.x, xy.y] = new GridNode(xy.x, xy.y, isWalkable(grid[xy]));
    }

    return gridNodes[xy.x, xy.y];
  }
  
}