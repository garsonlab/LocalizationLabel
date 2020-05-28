using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FontType
{
    Fangzheng,
    Fangzheng8,
    MH_ItX8,
    Minghei,
    Minghei8,
}

public class Label : Text
{
    [SerializeField] private bool m_UseLocalization;
    [SerializeField] private string m_LocalizationKey;
    [SerializeField] private FontType m_FontType;
    [SerializeField] private bool m_TiltEffect;
    [SerializeField] private float m_TiltAngle;
    private readonly UIVertex[] m_TempVerts = new UIVertex[4];
    private FontType m_LastFontType;
    private Font m_LastFont;

    /// <summary>是否使用多语言</summary>
    public bool useLocalization
    {
        get { return this.m_UseLocalization; }
        set
        {
            if(this.m_UseLocalization == value)
                return;

            this.m_UseLocalization = value;
            this.SetVerticesDirty();
            this.SetLayoutDirty();
        }
    }
    /// <summary>多语言Key</summary>
    public string localizationKey
    {
        get { return this.m_LocalizationKey; }
        set
        {
            if(this.m_LocalizationKey.Equals(value))
                return;

            this.m_LocalizationKey = value;

            if (this.m_UseLocalization)
            {
                this.SetVerticesDirty();
                this.SetLayoutDirty();
            }
        }
    }
    /// <summary>字体类型</summary>
    public FontType fontType
    {
        get { return this.m_FontType; }
        set
        {
            if(this.m_FontType == value)
                return;

            this.m_FontType = value;
            this.m_LastFontType = value;
            this.CheckFont();
        }
    }
    /// <summary>倾斜排列</summary>
    public bool tiltEffect
    {
        get { return this.m_TiltEffect; }
        set
        {
            if(this.m_TiltEffect == value)
                return;
            this.m_TiltEffect = value;
            this.SetLayoutDirty();
            this.SetVerticesDirty();
        }
    }
    /// <summary>倾斜角度</summary>
    public float tiltAngle
    {
        get { return this.m_TiltAngle; }
        set
        {
            if(this.m_TiltAngle == value)
                return;
            this.m_TiltAngle = value;
            if (this.m_TiltEffect)
            {
                this.SetLayoutDirty();
                this.SetVerticesDirty();
            }
        }
    }
    
    public override string text
    {
        get
        {
            if (this.m_UseLocalization)
                this.m_Text = FontManager.GetLanguage(this.m_LocalizationKey);

            return this.m_Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(this.m_Text))
                    return;
                this.m_Text = "";
                this.SetVerticesDirty();
            }
            else
            {
                if (!(this.m_Text != value))
                    return;
                this.m_Text = value;
                this.SetVerticesDirty();
                this.SetLayoutDirty();
            }
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        this.CheckFont();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if ((UnityEngine.Object) this.font == (UnityEngine.Object) null)
            return;
        this.m_DisableFontTextureRebuiltCallback = true;
        this.cachedTextGenerator.PopulateWithErrors(this.text, this.GetGenerationSettings(this.rectTransform.rect.size), this.gameObject);
        IList<UIVertex> verts = this.cachedTextGenerator.verts;
        float num1 = 1f / this.pixelsPerUnit;
        int num2 = verts.Count - 4;
        if (num2 <= 0)
        {
            toFill.Clear();
        }
        else
        {
            Vector2 point = new Vector2(verts[0].position.x, verts[0].position.y) * num1;
            Vector2 vector2 = this.PixelAdjustPoint(point) - point;
            toFill.Clear();
            if (vector2 != Vector2.zero)
            {
                float start = verts[0].position.y;
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = index1 & 3;
                    this.m_TempVerts[index2] = verts[index1];
                    this.m_TempVerts[index2].position *= num1;
                    this.m_TempVerts[index2].position.x += vector2.x;
                    this.m_TempVerts[index2].position.y += vector2.y;

                    if (index2 == 3)
                    {
                        if (this.m_TiltEffect)
                        {
                            float offset = Mathf.Tan(this.m_TiltAngle * Mathf.Deg2Rad) * (this.m_TempVerts[0].position.y-start);
                            for (int i = 0; i < 4; i++)
                            {
                                this.m_TempVerts[i].position.x += offset;
                            }
                        }
                        toFill.AddUIVertexQuad(this.m_TempVerts);
                    }
                }
            }
            else
            {
                float start = verts[0].position.y;
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int index2 = index1 & 3;
                    this.m_TempVerts[index2] = verts[index1];
                    this.m_TempVerts[index2].position *= num1;
                    if (index2 == 3)
                    {
                        if (this.m_TiltEffect)
                        {
                            float offset = Mathf.Tan(this.m_TiltAngle * Mathf.Deg2Rad) * (this.m_TempVerts[0].position.y-start);
                            for (int i = 0; i < 4; i++)
                            {
                                this.m_TempVerts[i].position.x += offset;
                            }
                        }
                        toFill.AddUIVertexQuad(this.m_TempVerts);
                    }
                }
            }
            this.m_DisableFontTextureRebuiltCallback = false;
        }
    }
    
    private void CheckFont()
    {
        var myFont = FontManager.GetFont(this.m_FontType);
        if (myFont != this.font)
        {
            this.font = myFont;
        }
    }
    
    protected override void OnValidate()
    {
        if (this.m_LastFontType != this.m_FontType || this.m_LastFont != font)
        {
            this.CheckFont();
            this.m_LastFontType = this.m_FontType;
            this.m_LastFont = font;
        }
        base.OnValidate();
    }
    
}
