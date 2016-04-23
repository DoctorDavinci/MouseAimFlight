﻿/*
Copyright (c) 2016, ferram4, tetryds
All rights reserved.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MouseAimFlight
{
    class MouseAimSettings
    {
        static MouseAimSettings instance;
        public static MouseAimSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new MouseAimSettings();

                return instance;
            }
        }

        static KeyCode toggleKeyCode = KeyCode.P;
        public static KeyCode ToggleKeyCode
        {
            get { return toggleKeyCode; }
        }
        static string toggleKeyString = "P";
        public static string ToggleKeyString
        {
            get { return toggleKeyString; }
            set
            {
                string tmp = value.ToUpperInvariant();
                if(tmp != toggleKeyString && tmp.Length == 1)
                {
                    toggleKeyString = tmp;
                    toggleKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), toggleKeyString);
                    Instance.SaveSettings();
                }
            }
        }

        public enum CursorStyle
        {
            FULL,
            DOT,
            NONE
        }
        static CursorStyle cursor = CursorStyle.FULL;
        public static CursorStyle Cursor
        {
            get { return cursor; }
            set
            {
                if (value != cursor)
                {
                    cursor = value;
                    Instance.SaveSettings();
                }
            }
        }

        static float mouseSensitivity = 100;
        public static float MouseSensitivity
        {
            get { return mouseSensitivity; }
            set
            {
                if (value != mouseSensitivity)
                {
                    mouseSensitivity = value;
                    Instance.SaveSettings();
                }
            }
        }

        static bool invertY = false;
        static bool invertX = false;
        public static bool InvertYAxis
        {
            get { return invertY; }
            set
            {
                if (value != invertY)
                {
                    invertY = value;
                    Instance.SaveSettings();
                }
            }
        }

        public static bool InvertXAxis
        {
            get { return invertX; }
            set
            {
                if (value != invertX)
                {
                    invertX = value;
                    Instance.SaveSettings();
                }
            }
        }

        static bool fARLoaded;
        public static bool FARLoaded
        {
            get { return fARLoaded; }
            set
            {
                fARLoaded = value;
            }
        }

        MouseAimSettings()
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            ConfigNode node = GameDatabase.Instance.GetConfigNode("MAFSettings");
            if((object)node != null)
            {
                if (node.HasValue("toggleKey"))
                {
                    toggleKeyString = ((string)node.GetValue("toggleKey")).ToUpperInvariant();
                    toggleKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), toggleKeyString);
                }
                if(node.HasValue("cursorStyle"))
                {
                    object temp = Enum.Parse(typeof(CursorStyle), (string)node.GetValue("cursorStyle"));
                    if (temp != null)
                        cursor = (CursorStyle)temp;
                }
                if (node.HasValue("mouseSensitivity"))
                {
                    float.TryParse(node.GetValue("mouseSensitivity"), out mouseSensitivity);
                }
                if(node.HasValue("invertX"))
                {
                    bool.TryParse(node.GetValue("invertX"), out invertX);
                }
                if (node.HasValue("invertY"))
                {
                    bool.TryParse(node.GetValue("invertY"), out invertX);
                }
            }
            DetectFARLoaded();
        }

        public void SaveSettings()
        {
            ConfigNode node = new ConfigNode("MAFSettings");
            node.AddValue("name", "default");
            node.AddValue("toggleKey", toggleKeyCode.ToString());
            node.AddValue("cursorStyle", cursor.ToString());
            node.AddValue("mouseSensitivity", mouseSensitivity.ToString());
            node.AddValue("invertX", invertX.ToString());
            node.AddValue("invertY", invertY.ToString());

            ConfigNode saveNode = new ConfigNode();
            saveNode.AddNode(node);
            saveNode.Save(KSPUtil.ApplicationRootPath.Replace("\\", "/") + "GameData/MouseAimFlight/MAFSettings.cfg");
        }

        void DetectFARLoaded()
        {
            foreach (AssemblyLoader.LoadedAssembly loaded in AssemblyLoader.loadedAssemblies)
            {
                if (loaded.assembly.GetName().Name == "FerramAerospaceResearch")
                {
                    FARLoaded = true;
                    break;
                }
                else
                    FARLoaded = false;
            }

            if(FARLoaded)
                Debug.Log("[MAF]: FAR loaded, switching adaptive gain settings");
            else
                Debug.Log("[MAF]: Stock aero model loaded, switching adaptive gain settings");
        }
    }
}
