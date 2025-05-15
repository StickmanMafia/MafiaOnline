using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    public Image[] flags;
    public MainMenuBehaviour menu;
    public Scroller scroller;
    public void Start(){
        
            menu.switchLang(PlayerPrefs.GetInt("language",0));

        CheckCurrentLanguage();
    }
    
    public void NewLanguage(int lan){
        menu.switchLang(lan);
        CheckCurrentLanguage();
        scroller.StartAndCheck();
    }
}
