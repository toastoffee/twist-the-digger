using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid<T> : IEnumerable<T> {
    private T[,] m_grid;
    public Vector2Int size => new Vector2Int(m_grid.GetLength(0), m_grid.GetLength(1));

    public Grid(int width, int height) {
        m_grid = new T[width, height];
    }

    public Grid(T[,] grid) {
        m_grid = grid;
    }

    public T this[int x, int y] {
        get {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y)
                throw new System.IndexOutOfRangeException();
            return m_grid[x, y];
        }
        set {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y)
                throw new System.IndexOutOfRangeException();
            m_grid[x, y] = value;
        }
    }
    
    public T this[Vector2Int pos] {
        get => this[pos.x, pos.y];
        set => this[pos.x, pos.y] = value;
    }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        for (int i = 0; i < m_grid.GetLength(0); i++) {
            for (int j = 0; j < m_grid.GetLength(1); j++) {
                yield return m_grid[i, j];
            }
        }
    }
    public IEnumerator GetEnumerator() {
        return m_grid.GetEnumerator();
    }
    

    public bool IsPositionValid(Vector2Int idx) {
        return IsPositionValid(idx.x, idx.y);
    }

    public bool IsPositionValid(int x, int y) {
        return x >= 0 && x < size.x && y >= 0 && y < size.y;
    }
  
    
    public enum ClampType {
        Lock,
        Loop
    }
    
    public Vector2Int Clamp(Vector2Int idx, ClampType type) {
        switch (type)
        {
            case ClampType.Lock:
                return new Vector2Int(
                    Mathf.Clamp(idx.x, 0, size.x - 1), 
                    Mathf.Clamp(idx.y, 0, size.y - 1));
            break;

            case ClampType.Loop:
                return new Vector2Int((idx.x + size.x)  % size.x, (idx.y + size.y) % size.y);
            break;
        }
        return idx;
    }

}
