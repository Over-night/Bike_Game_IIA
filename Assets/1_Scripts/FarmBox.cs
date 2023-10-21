using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Plante_state
{
    none, dust, plante
}

public class FarmBox : MonoBehaviour
{
    public Plante_state cur_plante_State = Plante_state.none;

    [Header("Plante")]
    public Image IMG_Plante;
    public List<Sprite> List_IMG_Plantes;

    [Header("Interact")]
    public GameObject BTN_Interact;
    public Image IMG_interact;
    public Text TXT_interact;
    public List<Sprite> List_IMG_interacts;

    // Start is called before the first frame update
    void Start()
    {
        IMG_Plante.enabled = false;   
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BTN_Set_Interact()
    {

        BTN_Interact.SetActive(false);
        StartCoroutine(CO_PlanteInteract());
    }

    IEnumerator CO_PlanteInteract()
    {
        // 대기 (다음번호로 변경)
        if((int)cur_plante_State >= 2)
        {
            cur_plante_State = Plante_state.none;
        }
        else
        {
            cur_plante_State++;
        }

        yield return new WaitForSeconds(2f);
        // 작물이미지 바꾸기
        switch (cur_plante_State)
        {
            case Plante_state.none:
                //수확완료 
                IMG_Plante.enabled = false;
                TXT_interact.text = "작물심기";
                break;

            case Plante_state.dust:
                IMG_Plante.enabled = true;
                IMG_Plante.sprite = List_IMG_Plantes[0];
                TXT_interact.text = "물주기";
                break;

            case Plante_state.plante:
                IMG_Plante.enabled = true;
                TXT_interact.text = "수확하기";
                IMG_Plante.sprite = List_IMG_Plantes[Random.Range(1,3)];
                break;
        }
        IMG_Plante.SetNativeSize();

        yield return new WaitForSeconds(2f);
        // 말풍선 띄우기

        BTN_Interact.SetActive(true);
        switch (cur_plante_State)
        {
            case Plante_state.none:
                IMG_interact.sprite = List_IMG_interacts[0];

                break;

            case Plante_state.dust:
                IMG_interact.sprite = List_IMG_interacts[1];

                break;

            case Plante_state.plante:
                IMG_interact.sprite = List_IMG_interacts[2];

                break;
        }
    }
}
