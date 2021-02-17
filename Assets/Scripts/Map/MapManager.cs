using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject[] localMaps = default;
    [SerializeField] GameObject worldMap = default;
    [SerializeField] GameObject player = default;
    // 最後に訪れた街:戻るときに必要
    // 戻る？？？　ローカルマップで新たに生成してやればいいのでは？
    // Playerも非表時のする？

    private void Start()
    {
        ShowWorldMap();
    }
    public void ShowWorldMap()
    {
        player.SetActive(true);
        worldMap.SetActive(true);
        HideLocalMap();
    }

    public void ShowLocalMap(int id)
    {
        player.SetActive(false);
        worldMap.SetActive(false);
        HideLocalMap();
        localMaps[id].SetActive(true);
    }


    void HideLocalMap()
    {
        // 全てのローカルマップを非表示
        foreach (GameObject localMap in localMaps)
        {
            localMap.SetActive(false);
        }
    }
}
