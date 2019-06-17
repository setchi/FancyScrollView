﻿using System;
using UnityEngine;

namespace FancyScrollView.Example04
{
    public class Context
    {
        public int SelectedIndex = -1;

        // Cell -> ScrollView
        public Action<int> OnCellClicked;

        // ScrollView -> Cell
        public Action UpdateCellState;

        // xy = cell position, z = data index, w = scale
        public Vector4[] CellState = new Vector4[1];

        public void SetCellState(int cellIndex, int dataIndex, float scale, Vector2 position)
        {
            var size = cellIndex + 1;
            ResizeIfNeeded(ref CellState, size);

            CellState[cellIndex].x = position.x;
            CellState[cellIndex].y = position.y;
            CellState[cellIndex].z = dataIndex;
            CellState[cellIndex].w = scale;
        }

        void ResizeIfNeeded<T>(ref T[] array, int size)
        {
            if (size <= array.Length)
            {
                return;
            }

            var tmp = array;
            array = new T[size];
            Array.Copy(tmp, array, tmp.Length);
        }
    }
}