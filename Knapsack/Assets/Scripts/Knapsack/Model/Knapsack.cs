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
        else {
            grid.AddItem(item);
        }
    }

    public void ExangeItem(Grid lhs, Grid rhs) {
        if(!lhs.IsEmpty() && !rhs.IsEmpty()) {
            Item litem = lhs.GetItem();
            Item ritem = rhs.GetItem();

            lhs.RemoveItem();//删除之前的item
            rhs.RemoveItem();//删除之前的item

            lhs.AddItem(ritem);
            rhs.AddItem(litem);
        }
    }

    private void InitGridList() {
        if (gridList == null) {
            gridList = new List<Grid>();
            List<GameObject> grids = new List<GameObject>(  GameObject.FindGameObjectsWithTag("Grid") );//获得的Grid并不是按照面板上显示的层级的顺序,需要排序
            //按照gird的序号升序排序
            grids.Sort((lhs, rhs) => int.Parse(lhs.transform.parent.name).CompareTo(int.Parse(rhs.transform.parent.name)));
            foreach (var grid in grids) {
                gridList.Add(grid.GetComponent<Grid>());
            }
        }

    }

}
