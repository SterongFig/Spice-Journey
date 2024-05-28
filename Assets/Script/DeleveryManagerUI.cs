using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleveryManagerUI : MonoBehaviour
{
    List<int> pos_card_menu_x = new() { 117, 331, 548, 765 };
    const int pos_card_menu_y = 967;
    public List<ScriptingMenu> m_Menu;
    public List<GameObject> o_Menu;
    [SerializeField] Canvas canvas;
    bool update = false;

    void Update()
    {
        if (update)
        {
            int i = 0;
            foreach (var item in o_Menu)
            {
                o_Menu[i].GetComponent<RectTransform>().position = new Vector3(pos_card_menu_x[i], pos_card_menu_y,0);
                i++;
            }
            update = false;
        }
        transform.position = new Vector3(958 , 538 , 0);
    }

    public bool AddRecepie(ScriptingMenu menuRecepie)
    {
        if (m_Menu.Count == 4)
        {
            return false; //the menu is not added because full
        }
        m_Menu.Add(menuRecepie);
        o_Menu.Add(CreateMenu(menuRecepie));
        update = true;
        return true; // the menu is added
    }

    public void DestroyMenu(int index) //jika menu diserved (benar) maka harus hilang
    {
        m_Menu.RemoveAt(index);
        Destroy(o_Menu[index].gameObject);
        o_Menu.RemoveAt(index);
        update = true;
    } 

    private GameObject CreateMenu(ScriptingMenu menuRecepie)
    {
        GameObject newGameObject = new GameObject(menuRecepie.name + "_menu");
        var rect = newGameObject.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(230, 230);
        newGameObject.AddComponent<CanvasRenderer>();
        var image = newGameObject.AddComponent<Image>();
        image.sprite = menuRecepie.billMenuSprite;
        // add bar and fill
        GameObject newBar = new GameObject("Bar");
        var rect2 = newBar.AddComponent<RectTransform>();
        rect2.sizeDelta = new Vector2(200, 26.5f);
        newBar.AddComponent<CanvasRenderer>();
        var image2 = newBar.AddComponent<Image>();
        image2.color = Color.white;
        GameObject newFill = new GameObject("Fill");
        var rect3 = newFill.AddComponent<RectTransform>();
        rect3.sizeDelta = new Vector2(190, 20);
        newFill.AddComponent<CanvasRenderer>();
        var image3 = newFill.AddComponent<Image>();
        image3.color = Color.green;
        newBar.transform.SetParent(newGameObject.transform);
        newFill.transform.SetParent(newGameObject.transform);
        newGameObject.transform.SetParent(transform); // this new menu is set to DeliverymanagerUI
        rect2.position = new Vector3(1.9f, 97.1f, 0);
        rect3.position = new Vector3(1.9f, 97.1f, 0);
        var controller = newGameObject.AddComponent<MenuController>();
        controller.bar = newBar;
        controller.fill = newFill;
        controller.scoreManager = canvas.GetComponent<ScoreManager>();
        return newGameObject;
    }
}
