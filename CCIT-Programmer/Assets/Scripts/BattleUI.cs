using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public string towerDirectoryPath = null;
    public GameObject installationTower = null;
    
    #region 임시 변수
    //데이터베이스에서 덱 정보 가져와야함
    public string[] deck = new string[5]{"COW", "GIRAFFE", "ELEPHANT", "LION", "BIRD"};
    #endregion

    void Start()
    {
        string a = aaa();
        if(a != null)
        {

        }
    }

    string aaa()
    {
        return null;
    }

    void Update()
    {
        //타워를 선택했다면 레이캐스트로 그리드 선택하여 생성
        if(installationTower != null)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if(hit.transform.name == "Grid")
                {
                    installationTower.transform.position = hit.transform.position + new Vector3(0f, 3.5f, 0f);
                }
            }
        }

#if UNITY_EDITOR
        Debug.Log("정처리기");
#elif UNITY_ANDROID
        Debug.Log("정처리기");
#elif UNITY_IOS
        Debug.Log("정처리기");
#endif
    }

    public void Button_InstallationTowerSelection(int index)
    {
        if (deck.Length - 1 < index) return;
        if (installationTower != null)
        {
            Destroy(installationTower);
            installationTower = null;
        }
        string towerPrefabPath = towerDirectoryPath + deck[index];
        Debug.Log(towerPrefabPath);
        GameObject obj = Resources.Load(towerPrefabPath) as GameObject;
        installationTower = Instantiate(obj);

    }
}
