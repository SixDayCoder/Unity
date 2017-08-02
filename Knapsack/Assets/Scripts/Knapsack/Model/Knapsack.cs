using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knapsack {

    #region singleton
    private Knapsack() {
        InitGridList();
    }
    private static Knapsack _instance = null;
    public  static Knapsack Instance {
        get {
            if (_instance == null)
                _instance = new Knapsack();
            return _instance;
        }
    }
    #endregion





    private List<Grid> gridList = null;


    private Grid FindFirstEmptyGrid() {
        if (gridList != null) {
            foreach (var grid in gridList) {
                if (grid.IsEmpty())
                    return grid;
            }
        }
        return null;
    }

    public void AddItem(Item item) {
        Grid grid = FindFirstEmptyGrid();
        if (grid == null) {
            //LogError(Knapsack is full)
        }
        grid.AddItem(item);
    }

    public void RemoveItem() {

    }

    public void LoadItems() {//从服务器获取json文件并解析

    }

    private void InitGridList() {
        if (gridList == null) {
            gridList = new List<Grid>();
            GameObject[] grids = GameObject.FindGameObjectsWithTag("Grid");//获得的Grid并不是按照面板上显示的层级的顺序,需要排序
            foreach (var grid in grids) {
                gridList.Add(grid.GetComponent<Grid>());
            }
            //按照gird的序号升序排序
            gridList.Sort((lhs, rhs) => int.Parse(lhs.name).CompareTo (int.Parse(rhs.name)) );
        }

    }

}
