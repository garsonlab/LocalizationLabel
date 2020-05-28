using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Framework.GalaSports.Language;
using UnityEngine;

public class FontManager
{
    static Dictionary<FontType, Font> _fonts = new Dictionary<FontType, Font>();
    
    public static void LoadFont()
    {
        
    }
    
    
    public static void Clear()
    {
        
    }


    public static Font GetFont(FontType fontType)
    {
        if (_fonts.Count <= 0)
        {
            foreach (var name in Enum.GetNames(typeof(FontType)))
            {
                var f = name.Replace('_', '-');
                var a = UnityEditor.AssetDatabase.LoadAssetAtPath<Font>($"Assets/LoadResources/Font/{f}.ttf");
                if(a == null)
                    a = UnityEditor.AssetDatabase.LoadAssetAtPath<Font>($"Assets/LoadResources/Font/{f}.TTF");

                FontType.TryParse(name, out FontType ft);
                _fonts.Add(ft, a);
            }
        }
        
        _fonts.TryGetValue(fontType, out Font font);
        return font;
    }

#if UNITY_EDITOR
    private static Dictionary<string, string> langs = new Dictionary<string, string>();
#endif

    public static string GetLanguage(string langkey)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (langs.Count <= 0)
            {
                var text = File.ReadAllText("Assets/LoadResourcesLanguage/CN_Folder/CN/Cfgs/Language.bytes");
                
                string key = String.Empty;
                StringUtils.Parse(text, (row, column, HeaderAttribute, val) =>
                {
                    if (column == 0)
                        key = val;
                    else if (column == 1)
                    {
                        if(langs.ContainsKey(key))
                            Debug.LogError("Duplicate Key In Language, Key = " + key);
                        else
                            langs.Add(key, val);
                    }
                }, null, false);
            }

            if (langs.TryGetValue(langkey, out string lang))
                return lang;
            return langkey;
        }
#endif
        return LanguageService.Instance.GetStringByKey(langkey);
    }

    public static FontType GetType(Font font)
    {
        string name = font.name.Replace('-', '_');
        FontType type = (FontType)Enum.Parse(typeof(FontType), name);
        return type;
    }
}
