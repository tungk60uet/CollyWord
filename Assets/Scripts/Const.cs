using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const {
    public static List<String> colorHex = new List<String> { "#FC111A", "#030103", "#F9FEFA", "#FEFD49", "#E490B4", "#591802", "#7C1893", "#ff6501", "#15A02B", "#2BB2FC" };
    public static List<String> colorBgr = new List<String> { "#F8DBDB", "#FEF7FF", "#2F1933", "#2F1933", "#F9E6E8", "#915e44", "#EAC7F1", "#F2EAD6", "#DFDCA3", "#E8EAF8" };
    public static List<String> colorName;
    
    public static String accessToken= "";
    public static String listFriend = "";
    public static void initGameData()
    {
        Hashtable hashtable = new Hashtable();
        List<String> vn = new List<String> { "ĐỎ", "ĐEN", "TRẮNG", "VÀNG", "HỒNG", "NÂU", "TÍM", "DA CAM", "XANH LÁ CÂY", "XANH DA TRỜI" };
        List<String> en = new List<String> { "RED", "BLACK", "WHITE", "YELLOW", "PINK", "BROWN", "VIOLET", "ORANGE", "GREEN", "BLUE" };
        List<String> cn = new List<String> { "红色", "黑色", "白色", "黄色", "粉红色", "棕色", "紫色", "橙色", "紫色", "天蓝色" };
        List<String> id = new List<String> { "MERAH", "HITAM", "PUTIH", "KUNING", "MERAH MUDA", "COKLAT", "UNGU", "JERUK", "HIJAU", "BIRU" };
        List<String> jp = new List<String> { "赤", "黒", "白", "黄", "ピンク", "褐色", "紫", "オレンジ", "緑", "青" };
        List<String> kr = new List<String> { "빨간색", "까만색", "하얀색", "노란색", "분홍색", "밤색", "보라색", "오렌지색", "녹색", "파란색" };
        List<String> ru = new List<String> { "красный", "чёрный", "белый", "жёлтый", "розовый", "коричневый", "фиолетовый", "оранжевый", "зелёный", "синий" };
        List<String> th = new List<String> { "สีแดง", "สีดำ", "ขาว", "สีเหลือง", "สีชมพู", "สีน้ำตาล", "สีม่วง", "สีส้ม", "สีเขียว", "สีน้ำเงิน" };
        
        hashtable.Add("English", en);
        hashtable.Add("Vietnamese", vn);
        hashtable.Add("ChineseSimplified", cn);
        hashtable.Add("ChineseTraditional", cn);
        hashtable.Add("Chinese", cn);
        hashtable.Add("Indonesian", id);
        hashtable.Add("Japanese", jp);
        hashtable.Add("Korean", kr);
        hashtable.Add("Russian", ru);
        hashtable.Add("Thai", th);
        hashtable.Add("Unknown", en);
        colorName =hashtable[Application.systemLanguage.ToString()] as List<String>;

    }
}
