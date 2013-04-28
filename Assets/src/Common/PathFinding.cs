using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinding  {

	/*________________________________________________________________*/
	//	STATIC INITIALITZATION OF PATHFINDING
	/*________________________________________________________________*/	
	public const int COLS = 200;
	public const int ROWS = 200;
	public const float X_INITIAL = -150f;
	public const float Y_INITIAL = -150f;
	public const float CELL_WIDTH = 1.5f;
	public const float CELL_HEIGHT = 1.5f;
	
	// ---------------------------------------------------
	/**
	 * Static initialitzation of pathfinding, but also it can be called externally to adjust 
	 * to new parameters
	 */
	public static PathFinding InitPathfinding(int rows, int cols, 
	                                          float xIni, float zIni, 
	                                          float cellWidth, float cellHeight) 
	{
		int[] matrix = new int[rows * cols];
		Global.MakeMatrix(false, matrix, xIni, zIni, cellWidth, cellHeight, rows, cols);

		// PATHFINDING
		PathFinding path = new PathFinding(rows, cols, cellWidth, cellHeight, xIni, zIni);
		path.InitAI(matrix);
		return (path);
	}
	
	// INIT BY DEFAULT
	public static PathFinding m_path = InitPathfinding(ROWS, COLS, X_INITIAL, Y_INITIAL, CELL_WIDTH, CELL_HEIGHT);
	
	/*________________________________________________________________*/
	//	STATIC&CONSTANTS
	/*________________________________________________________________*/
	public const bool DEBUG_PATHFINDING= false;
	
	// ++ STATES ++
	public const int CELL_EMPTY		 	= 0;
	public const int CELL_COLLISION	 	= 1;
	
	/*________________________________________________________________*/
	//	PRIVATE MEMBERS
	/*________________________________________________________________*/		
	private static int m_cols;						//! Cols of the matrix
	private static int m_rows;						//! Rows of the matrix
	private static int m_totalCells;				//! Total number of cells
	private static float m_cellWidth;				//! Width of the cell
	private static float m_cellHeight;				//! Height of the cell
	private static float m_xIni;					//! Initial shift X
	private static float m_zIni;					//! Initial shift Z
	
	private static int[] m_cells;				//! List of cells to apply the pathfinding
	
	private static List<NodePath> m_matrixAI;	
	
	/*________________________________________________________________*/
	//	CONSTRUCTOR
	/*________________________________________________________________*/		
	// ---------------------------------------------------
	/**
	 * Constructor of cPathFinding
	 */
	public PathFinding(int cols, int rows, float cellWidth, float cellHeight, float xIni, float zIni)
	{
		m_cols = cols;
		m_rows = rows;
		m_totalCells = m_cols * m_rows;
		m_cellWidth = cellWidth;
		m_cellHeight = cellHeight;
		m_xIni = xIni;
		m_zIni = zIni;
					
		m_cells = new int[m_totalCells];
		for (int y = 0; y < m_rows; y++)			
		{
			for (int x = 0; x < m_cols; x++)
			{
				m_cells[(y * m_cols) + x] = CELL_COLLISION;					
			}
		}
	}

	// ---------------------------------------------------
	/**
	 * Will remake the whole matrix of collision
	 */
	public static void RemakePathfinding()
	{
		m_path = InitPathfinding(ROWS, COLS, X_INITIAL, Y_INITIAL, CELL_WIDTH, CELL_HEIGHT);
	}
	
	// ---------------------------------------------------
	/**
	 * Initialitzation of the matrix for making the search
	 */
	public void InitAI(int[] initContent)
	{			
		int i=0;
	
		for (int y = 0; y < m_rows; y++)
		{
			for (int x = 0; x < m_cols; x++)
			{
				int cellContent = initContent[(y * m_cols) + x];
				m_cells[(y * m_cols) + x] = cellContent;
			}
		}
		
		
		m_matrixAI = new List<NodePath>();
		for (i = 0; i < m_totalCells; i++)			
		{
			m_matrixAI.Add(new NodePath());
		}
	}
	
	// ---------------------------------------------------
	/**
	 * Render the position of points
	 */
	public static void RenderDebug()
	{		
		for (int y = 0; y < m_rows; y++)
		{
			for (int x = 0; x < m_cols; x++)
			{
				int cellContent = m_cells[(y * m_cols) + x];
				if (cellContent == CELL_COLLISION)
				{
					Debug.DrawLine( new Vector3((float)((x * m_cellWidth)-0.5)+m_xIni, 1f, (float)((y * m_cellHeight)-0.5)+m_zIni)
					               , new Vector3((float)((x * m_cellWidth)+0.5)+m_xIni, 1f, (float)((y * m_cellHeight)+0.5)+m_zIni)
					               , Color.red);
				}
				else
				{
					Debug.DrawLine( new Vector3((float)((x * m_cellWidth)-0.5)+m_xIni, 1f, (float)((y * m_cellHeight)-0.5)+m_zIni)
					               , new Vector3((float)((x * m_cellWidth)+0.5)+m_xIni, 1f, (float)((y * m_cellHeight)+0.5)+m_zIni)
					               , Color.blue);
				}
			}
		}
	}
		
	
	/*________________________________________________________________*/
	//	GETTERS/SETTERS
	/*________________________________________________________________*/
	// GETTERS
	public static int Cols
    {
        get { return m_cols; }
        set { m_cols = value; }
    }	
	public static int Rows
    {
        get { return m_rows; }
        set { m_rows = value; }
    }
	public static int TotalCells
    {
        get { return m_totalCells; }
        set { m_totalCells = value; }
    }
	public static float CellWidth
    {
        get { return m_cellWidth; }
        set { m_cellWidth = value; }
    }
	public static float CellHeight
    {
        get { return m_cellHeight; }
        set { m_cellHeight = value; }
    }
	public static float xIni
    {
        get { return m_xIni; }
        set { m_xIni = value; }
    }
	public static float zIni
    {
        get { return m_zIni; }
        set { m_zIni = value; }
    }
	
	/*________________________________________________________________*/
	//	PUBLIC FUNCTIONS
	/*________________________________________________________________*/
	// ---------------------------------------------------
	/**
	 * Gets the direction to go from two points
	*/  
	private static int GetDirectionByPosition( int x_o, int y_o, int x_d, int y_d )
	{ 
	  if (y_o > y_d) return (Global.DIRECTION_UP);		  
	  if (y_o < y_d) return (Global.DIRECTION_DOWN);		  
	  if (x_o < x_d) return (Global.DIRECTION_RIGHT);		  
	  if (x_o > x_d) return (Global.DIRECTION_LEFT);		  
	
	  return (Global.DIRECTION_NONE);
	}

	// ---------------------------------------------------
	/**
	 * Get the content of the cell in the asked position
	 */
	public static int GetCellContent(int x, int y)
	{
		if (y>=m_rows) return (CELL_COLLISION);
		if (x>=m_cols) return (CELL_COLLISION);
		return (int)(m_cells[(y * m_cols) + x]);
	}

	// ---------------------------------------------------
	/**
	 * Set the content of the cell in the asked position
	 */
	public static void SetCellContent(int x, int y, int val)
	{
		m_cells[(y * m_cols) + x] = val;			
	}
	
	// ---------------------------------------------------
	/**
	 * Distance between two points
	*/  
	private static float GetDistance( int x_o, int y_o, int x_d, int y_d)
	{ 
	  return (Mathf.Abs(x_o - x_d) + Math.Abs(y_o - y_d)); 
	}

	// ---------------------------------------------------
	/**
	 * Test if the child generated is correct
	*/  
	private static int GetCorrectChild(int pos_x, int pos_y, int sizeMatrix)		
	{
		int sCell;
		int i;
				
		// Check valid cell
		if ((pos_x<0)||(pos_x>=m_cols)) return (0);							
		if ((pos_y<0)||(pos_y>=m_rows))  return (0);			
	
		sCell = GetCellContent(pos_x, pos_y);
		// Debug.Log("cPathFinding.as::GetCorrectChild(" + pos_x + "," + pos_y + ")=" + sCell);			
		if (sCell == CELL_EMPTY)			
		{
			// Check if the cell has been checked
			for (i = 0; i <= sizeMatrix; i++)				
			{
			   if ((m_matrixAI[i].m_x == pos_x) && (m_matrixAI[i].m_y == pos_y))				   
				   return (0);
			}
			// Debug.Log("cPathFinding.as::GetCorrectChild IS EMPTY");			
			return (1);
		}
		else
		{
			return (0);
		}		
	}

	// ---------------------------------------------------
	/**
	* Do the search A* in the matrix to search a type or a position
	* @param x_ori	Initial position X
	* @param y_ori	Initial position Y
	* @param x_des	Final position X
	* @param y_des	Final position Y
	*/ 
	public static Vector2 SearchAStar( int x_ori, int y_ori, int x_des, int y_des)		
	{
		 int i;
		 int j;
		 int numCellsGenerated;
		 int directionInitial;
		 int posx;
		 int posy;
		 int sizeMatrix;
		 float minimalValue;
		 int sbusq;
		 
		// Debug.DrawLine(new Vector3((x_ori * m_cellWidth)+m_xIni , 1, (y_ori * m_cellHeight)+m_zIni)
		//               , new Vector3((x_des* m_cellWidth)+m_xIni, 1, (y_des * m_cellHeight)+m_zIni)
		//               , Color.black);
		
		if (DEBUG_PATHFINDING)
		{
			Debug.Log("cPathFinding.as::SearchAStar:: ORIGIN(" + x_ori + "," + y_ori + "); DESTINATION(" + x_des + "," + y_des + "); COLUMNS=" + m_cols + ";ROWS=" + m_rows);
			Debug.Log("CONTENT=" + m_cells);			 
		}
		 
		// SAME POSITION
		if ((x_ori == x_des) && (y_ori == y_des))
		{
			return (new Vector2(x_ori, y_ori));
		}
		
		// CHECK VALID ORIGIN AND GOAL
		if ((GetCellContent(x_ori,y_ori)==CELL_COLLISION)||(GetCellContent(x_des,y_des)==CELL_COLLISION))
		{
			return (new Vector2(-1f, -1f));
		}
		
		 // Init search matrix
		 for (i = 0; i < m_totalCells; i++)			 
		 {
			m_matrixAI[i].m_x = -1;  		// x				
			m_matrixAI[i].m_y = -1;  		// y				
			m_matrixAI[i].m_directionInitial = Global.DIRECTION_NONE; 	// initialDirection				
			m_matrixAI[i].m_hasBeenVisited = -1; 	// checked				
			m_matrixAI[i].m_value = 10000;			// value			
			m_matrixAI[i].m_previousCell = -1;		// previous_cell				
		 }
	
		 // Init initial cell
		 sizeMatrix = 0;			 
		 m_matrixAI[sizeMatrix].m_x = x_ori;  // x			 
		 m_matrixAI[sizeMatrix].m_y  = y_ori;  // y			 
		 m_matrixAI[sizeMatrix].m_hasBeenVisited = 1;	   // checked			 
		 m_matrixAI[sizeMatrix].m_directionInitial = Global.DIRECTION_NONE; // initialDirection			 
		 if ((x_des == -1) && (y_des == -1))			 
		 {
			m_matrixAI[sizeMatrix].m_value = 0; 
		 }
		 else
		 {
			m_matrixAI[sizeMatrix].m_value = GetDistance(x_ori, y_ori, x_des, y_des); 		   
		 }
		 m_matrixAI[sizeMatrix].m_previousCell=-1;
		  
		 // ++ Init search ++
		 // Look through all the list looking for the goal and creating new childs
		 i=0;	 
		 do
		 {
			numCellsGenerated=0;

			// ++ Control the maximum lenght of the serach ++
			if (sizeMatrix > m_totalCells - 5)
			{
				if (DEBUG_PATHFINDING) Debug.Log("cPathFinding.as::SearchAStar:: RETURN 1");
				return (new Vector2(-1f, -1f));
			}
	
			// ++ Buscamos el primer nodo minimo a reproducir ++
			minimalValue = 100000000;
			i = -1;
			for (j = 0; j <= sizeMatrix; j++)
			{
			  if (m_matrixAI[j].m_hasBeenVisited == 1) // checked				  
			  {
				if (m_matrixAI[j].m_value < minimalValue)
				{
					i = j;
					minimalValue = m_matrixAI[j].m_value;
				}
			  }
			}

			if (i == -1)
			{
				if (DEBUG_PATHFINDING) Debug.Log("cPathFinding.as::SearchAStar:: RETURN 2");
				return (new Vector2(-1f, -1f));
			}

			// ++ Select a cell ++
			sbusq = i;

			// Debug.Log("COMPARANDO=("+m_matrixAI[i].m_x+","+m_matrixAI[i].m_y+") RESPECTO DESTINO=("+x_des+","+y_des+")");  				
			if ((m_matrixAI[i].m_x == x_des) && (m_matrixAI[i].m_y == y_des))				
			{
				// CREATE THE LIST OF CELLS BETWEEN DESTINATION-ORIGIN
				if (i == -1)
				{
					return (new Vector2(x_des, y_des));
				}
				else
				{					
					int curIndexBack = i;
					Vector2 sGoalNext = new Vector2(-1f, -1f);
					Vector2 sGoalCurrent = new Vector2(0f, 0f);
					do
					{
						
						sGoalCurrent.x = m_matrixAI[curIndexBack].m_x;
						sGoalCurrent.y = m_matrixAI[curIndexBack].m_y;
						
						if ((sGoalNext.x != -1)&&(sGoalNext.y != -1))
						{
							Debug.DrawLine( new Vector3((sGoalNext.x * m_cellWidth)+m_xIni, 1f, (sGoalNext.y * m_cellWidth)+m_zIni)
						               		, new Vector3((sGoalCurrent.x * m_cellWidth)+m_xIni, 1f, (sGoalCurrent.y * m_cellHeight)+m_zIni)
						               		, Color.black);
						}
						
						sGoalNext.x = sGoalCurrent.x;
						sGoalNext.y = sGoalCurrent.y;
						
						curIndexBack = m_matrixAI[curIndexBack].m_previousCell;								
					} while ((curIndexBack != 0)&&(curIndexBack != -1));			
					if (DEBUG_PATHFINDING) Debug.Log("cPathFinding.as::SearchAStar:: RETURN 3:sGoalNext=" + sGoalNext.ToString());					
					return (sGoalNext);
				}
			}
			   
			// Marca de nodo checked
			m_matrixAI[sbusq].m_hasBeenVisited = 0; // checked				
			
			// Generation of Childs 
			//  Child UP
			posx = (int)(m_matrixAI[i].m_x);				
			posy = (int)(m_matrixAI[i].m_y + 1);				
			if (GetCorrectChild(posx, posy, sizeMatrix) == 1)				
			{ 
			   if (m_matrixAI[sbusq].m_directionInitial == Global.DIRECTION_NONE)
			   {
				   directionInitial = Global.DIRECTION_DOWN;
			   }
			   else
			   {
				   directionInitial = m_matrixAI[sbusq].m_directionInitial;
			   }
					   
			   sizeMatrix++;				   
			   m_matrixAI[sizeMatrix].m_x = posx;
			   m_matrixAI[sizeMatrix].m_y = posy;
			   m_matrixAI[sizeMatrix].m_hasBeenVisited = 1;
			   m_matrixAI[sizeMatrix].m_directionInitial = directionInitial;
			   if ((x_des == Global.DIRECTION_NONE) && (y_des == Global.DIRECTION_NONE))
			   {
					m_matrixAI[sizeMatrix].m_value = 0; 
			   }
			   else
			   {
					m_matrixAI[sizeMatrix].m_value = GetDistance(posx, posy, x_des, y_des); 		   
			   }
			   m_matrixAI[sizeMatrix].m_previousCell = i;				   
			   numCellsGenerated++;
			}

			if (DEBUG_PATHFINDING) Debug.Log("cPathFinding.as::SearchAStar:: ANALIZING(" + m_matrixAI[i].m_x + "," + m_matrixAI[i].m_y + ")");				
			
			// Child DOWN	
			posx = (int)(m_matrixAI[i].m_x);
			posy = (int)(m_matrixAI[i].m_y - 1);
			if (GetCorrectChild(posx, posy, sizeMatrix) == 1)				
			{ 
			   if (m_matrixAI[sbusq].m_directionInitial == Global.DIRECTION_NONE)
			   {
				   directionInitial = Global.DIRECTION_UP;
			   }
			   else
			   {
				   directionInitial = m_matrixAI[sbusq].m_directionInitial;
			   }
				   
			   sizeMatrix++;
			   m_matrixAI[sizeMatrix].m_x = posx;				   
			   m_matrixAI[sizeMatrix].m_y = posy;				   
			   m_matrixAI[sizeMatrix].m_hasBeenVisited = 1;				   
			   m_matrixAI[sizeMatrix].m_directionInitial = directionInitial;				   
			   if ((x_des == Global.DIRECTION_NONE) && (y_des == Global.DIRECTION_NONE))
			   {
					m_matrixAI[sizeMatrix].m_value = 0; 			   
			   }
			   else
			   {
					m_matrixAI[sizeMatrix].m_value = GetDistance(posx, posy, x_des, y_des); 
			   }
			   m_matrixAI[sizeMatrix].m_previousCell=i;
			   numCellsGenerated++;
			}
		
			//  Child LEFT
			posx = (int)(m_matrixAI[i].m_x - 1);				
			posy = (int)(m_matrixAI[i].m_y);				
			if (GetCorrectChild(posx, posy, sizeMatrix) == 1)				
			{ 
			   if (m_matrixAI[sbusq].m_directionInitial == Global.DIRECTION_NONE)
			   {
				   directionInitial = Global.DIRECTION_LEFT;
			   }
			   else
			   {
				   directionInitial = m_matrixAI[sbusq].m_directionInitial;
			   }
		
			   sizeMatrix++;
			   m_matrixAI[sizeMatrix].m_x = posx;				   
			   m_matrixAI[sizeMatrix].m_y = posy;				   
			   m_matrixAI[sizeMatrix].m_hasBeenVisited = 1;				   
			   m_matrixAI[sizeMatrix].m_directionInitial = directionInitial;				   
			   if ((x_des == Global.DIRECTION_NONE) && (y_des == Global.DIRECTION_NONE))
			   {
					m_matrixAI[sizeMatrix].m_value = 0; 			   
			   }
			   else
			   {
					m_matrixAI[sizeMatrix].m_value = GetDistance(posx, posy, x_des, y_des); 		   
			   }
			   m_matrixAI[sizeMatrix].m_previousCell = i;				   
			   numCellsGenerated++;
			}
	
			//  Child RIGHT	
			posx = (int)(m_matrixAI[i].m_x + 1);				
			posy = (int)(m_matrixAI[i].m_y);				
			if (GetCorrectChild(posx, posy, sizeMatrix) == 1)				
			{  
				if (m_matrixAI[sbusq].m_directionInitial == Global.DIRECTION_NONE)
				{
					directionInitial = Global.DIRECTION_RIGHT;
				}
				else
				{
					directionInitial = m_matrixAI[sbusq].m_directionInitial;
				}

				sizeMatrix++;
				m_matrixAI[sizeMatrix].m_x = posx;				  
				m_matrixAI[sizeMatrix].m_y = posy;				  
				m_matrixAI[sizeMatrix].m_hasBeenVisited = 1;				  
				m_matrixAI[sizeMatrix].m_directionInitial = directionInitial;				  
				if ((x_des == Global.DIRECTION_NONE) && (y_des == Global.DIRECTION_NONE))
				{
					m_matrixAI[sizeMatrix].m_value = 0; 			   
				}
				else
				{
					m_matrixAI[sizeMatrix].m_value = GetDistance(posx, posy, x_des, y_des); 		   
				}
				m_matrixAI[sizeMatrix].m_previousCell = i;
				numCellsGenerated++;
			}

	  } while (true);
	
	  if (DEBUG_PATHFINDING) Debug.Log("cPathFinding.as::SearchAStar:: RETURN 4");
	  return (new Vector2(-1f, -1f));
	}
}
	
