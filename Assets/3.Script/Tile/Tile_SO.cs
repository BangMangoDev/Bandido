using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_SO : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    private Tile tile;
    private Hand hand;

    private void Awake()
    {
        tile = FindObjectOfType<Tile>();
        hand = FindObjectOfType<Hand>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = transform.localScale * 0.5f;
        // 클릭한 타일을 current_tile로 설정
        if (tile.current_tile == null)
        {
            hand.hand_list.Remove(gameObject);
            tile.current_tile = gameObject;
            tile.InitRotate();
            tile.UpdateTileOffset();
            this.enabled=false;
        }
        else
        {
            hand.hand_list.Remove(gameObject);
            hand.hand_list.Add(tile.current_tile);
            tile.current_tile.transform.rotation = this.transform.rotation;
            tile.current_tile.GetComponent<Tile_SO>().enabled = true;
            tile.current_tile = gameObject;
            tile.InitRotate();
            tile.UpdateTileOffset();
            this.enabled = false;
            
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스를 타일에 올렸을 때 수행할 작업
        this.gameObject.transform.localScale=this.transform.localScale * 2;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = this.transform.localScale * 0.5f;
    }
}
