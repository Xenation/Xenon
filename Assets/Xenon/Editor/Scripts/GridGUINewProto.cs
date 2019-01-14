using UnityEngine;
using UnityEditor;

namespace Xenon.Editor.Exp {
	public class GridCell {

		public delegate string StringContentProvider();
		public StringContentProvider StringContent;

		public delegate GUIContent ContentProvider();
		public ContentProvider Content;
		
		public delegate float WidthProvider();
		public WidthProvider Width;

		public GridCell(StringContentProvider strProv) {
			StringContent = strProv;
		}
		
		public Rect DrawGUI(Rect rect) {
			if (Content != null) {
				// TODO code to draw GUI Content here
				EditorGUI.LabelField(rect, Content());
			} else if (StringContent != null) {
				EditorGUI.LabelField(rect, StringContent());
			}
			rect.x += rect.width;
			return rect;
		}
		
	}
	
	public class GridGUI {
		
		public enum ColumnWidthMode {
			ManualFixed,
			ManualWeighted,
			AutoFit
		}

		public enum HeaderType {
			Top,
			Left,
			Right,
			Bottom,
		}
		
		private HeaderType headerType;
		private GridCell[,] grid;
		private int countWidth;
		private int countHeight;

		public float lineHeight = 16f;
		public ColumnWidthMode columnWidthMode = ColumnWidthMode.AutoFit;
		public float[] columnWidths;
		public float[] columnWeights;

		//public void SetHeader(HeaderType hType, string[] labels) {
		//	header = new GridCell[labels.Length];
		//	headerType = hType;
		//	for (int i = 0; i < labels.Length; i++) {
		//		header[i] = new GridCell(() => { return labels[i]; });
		//	}
		//}

		public void SetColumnSizes(ColumnWidthMode mode, float[] sizes) {
			switch (columnWidthMode) {
				case ColumnWidthMode.ManualFixed:
					columnWidths = sizes;
					break;
				case ColumnWidthMode.ManualWeighted:
					columnWidths = new float[sizes.Length];
					columnWeights = new float[sizes.Length];
					float weightSum = 0f;
					for (int i = 0; i < sizes.Length; i++) {
						weightSum += sizes[i];
					}
					for (int i = 0; i < sizes.Length; i++) {
						columnWeights[i] = sizes[i] / weightSum;
					}
					break;
				case ColumnWidthMode.AutoFit:
					columnWidths = new float[sizes.Length];
					for (int x = 0; x < countWidth; x++) {
						float maxWidth = 0f;
						for (int y = 0; y < countHeight; y++) {
							float width = grid[y, x].Width();
							if (width > maxWidth) {
								maxWidth = width;
							}
						}
						columnWidths[x] = maxWidth;
					}
					break;

				default:
					break;
			}
		}
		
		public void DrawGUI(Rect rect) {
			// Weighted Mode prepass
			switch (columnWidthMode) {
				case ColumnWidthMode.ManualWeighted:
					for (int x = 0; x < countWidth; x++) {
						columnWidths[x] = columnWeights[x] * rect.width;
					}
					break;
			}
			
			// Draw
			Rect lineRect = new Rect(rect.x, rect.y, rect.width, lineHeight);
			for (int y = 0; y < countHeight; y++) {
				Rect cellRect = new Rect(lineRect.x, lineRect.y, columnWidths[y], lineRect.height);
				for (int x = 0; x < countWidth; x++) {
					cellRect.width = columnWidths[y];
					grid[y,x].DrawGUI(cellRect);
					cellRect.x += cellRect.width;
				}
				lineRect.y += lineHeight;
			}
		}
		
	}
}
