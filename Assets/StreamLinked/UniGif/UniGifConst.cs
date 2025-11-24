/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Text;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;

namespace ScoredProductions.StreamLinked.UniGif {
    public static partial class UniGif {
        /// <summary>
        /// Gif Texture
        /// </summary>
        public class GifTexture {
            // Texture
            public Texture2D m_texture2d;
            // Delay time until the next texture.
            public float m_delaySec;

            public GifTexture(Texture2D texture2d, float delaySec) {
				this.m_texture2d = texture2d;
				this.m_delaySec = delaySec;
            }
        }

        /// <summary>
        /// GIF Data Format
        /// </summary>
        private struct GifData {
            // Signature
            public byte m_sig0, m_sig1, m_sig2;
            // Version
            public byte m_ver0, m_ver1, m_ver2;
            // Logical Screen Width
            public ushort m_logicalScreenWidth;
            // Logical Screen Height
            public ushort m_logicalScreenHeight;
            // Global Color Table Flag
            public bool m_globalColorTableFlag;
            // Color Resolution
            public int m_colorResolution;
            // Sort Flag
            public bool m_sortFlag;
            // Size of Global Color Table
            public int m_sizeOfGlobalColorTable;
            // Background Color Index
            public byte m_bgColorIndex;
            // Pixel Aspect Ratio
            public byte m_pixelAspectRatio;
            // Global Color Table
            public List<byte[]> m_globalColorTable;
            // ImageBlock
            public List<ImageBlock> m_imageBlockList;
            // GraphicControlExtension
            public List<GraphicControlExtension> m_graphicCtrlExList;
            // Comment Extension
            public List<CommentExtension> m_commentExList;
            // Plain Text Extension
            public List<PlainTextExtension> m_plainTextExList;
            // Application Extension
            public ApplicationExtension m_appEx;
            // Trailer
            public byte m_trailer;

            public readonly string Signature
            {
                get
                {
                    char[] c = { (char)this.m_sig0, (char)this.m_sig1, (char)this.m_sig2 };
                    return new string(c);
                }
            }

            public readonly string Version
            {
                get
                {
                    char[] c = { (char)this.m_ver0, (char)this.m_ver1, (char)this.m_ver2 };
                    return new string(c);
                }
            }

            public readonly void Dump() {
				DebugManager.LogMessage("GIF Type: " + this.Signature + "-" + this.Version);
				DebugManager.LogMessage("Image Size: " + this.m_logicalScreenWidth + "x" + this.m_logicalScreenHeight);
                DebugManager.LogMessage("Animation Image Count: " + this.m_imageBlockList.Count);
                DebugManager.LogMessage("Animation Loop Count (0 is infinite): " + this.m_appEx.LoopCount);
                if (this.m_graphicCtrlExList != null && this.m_graphicCtrlExList.Count > 0) {
					StringBuilder sb = new StringBuilder("Animation Delay Time (1/100sec)");
                    for (int i = 0; i < this.m_graphicCtrlExList.Count; i++) {
                        sb.Append(", ");
                        sb.Append(this.m_graphicCtrlExList[i].m_delayTime);
                    }
                    DebugManager.LogMessage(sb.ToString());
                }
                DebugManager.LogMessage("Application Identifier: " + this.m_appEx.ApplicationIdentifier);
                DebugManager.LogMessage("Application Authentication Code: " + this.m_appEx.ApplicationAuthenticationCode);
            }
        }

        /// <summary>
        /// Image Block
        /// </summary>
        private struct ImageBlock {
            // Image Separator
            public byte m_imageSeparator;
            // Image Left Position
            public ushort m_imageLeftPosition;
            // Image Top Position
            public ushort m_imageTopPosition;
            // Image Width
            public ushort m_imageWidth;
            // Image Height
            public ushort m_imageHeight;
            // Local Color Table Flag
            public bool m_localColorTableFlag;
            // Interlace Flag
            public bool m_interlaceFlag;
            // Sort Flag
            public bool m_sortFlag;
            // Size of Local Color Table
            public int m_sizeOfLocalColorTable;
            // Local Color Table
            public List<byte[]> m_localColorTable;
            // LZW Minimum Code Size
            public byte m_lzwMinimumCodeSize;
            // Block Size & Image Data List
            public List<ImageDataBlock> m_imageDataList;

            public struct ImageDataBlock {
                // Block Size
                public byte m_blockSize;
                // Image Data
                public byte[] m_imageData;
            }
        }

        /// <summary>
        /// Graphic Control Extension
        /// </summary>
        private struct GraphicControlExtension {
            // Extension Introducer
            public byte m_extensionIntroducer;
            // Graphic Control Label
            public byte m_graphicControlLabel;
            // Block Size
            public byte m_blockSize;
            // Disposal Mothod
            public ushort m_disposalMethod;
            // Transparent Color Flag
            public bool m_transparentColorFlag;
            // Delay Time
            public ushort m_delayTime;
            // Transparent Color Index
            public byte m_transparentColorIndex;
            // Block Terminator
            public byte m_blockTerminator;
        }

        /// <summary>
        /// Comment Extension
        /// </summary>
        private struct CommentExtension {
            // Extension Introducer
            public byte m_extensionIntroducer;
            // Comment Label
            public byte m_commentLabel;
            // Block Size & Comment Data List
            public List<CommentDataBlock> m_commentDataList;

            public struct CommentDataBlock {
                // Block Size
                public byte m_blockSize;
                // Image Data
                public byte[] m_commentData;
            }
        }

        /// <summary>
        /// Plain Text Extension
        /// </summary>
        private struct PlainTextExtension {
            // Extension Introducer
            public byte m_extensionIntroducer;
            // Plain Text Label
            public byte m_plainTextLabel;
            // Block Size
            public byte m_blockSize;
            // Block Size & Plain Text Data List
            public List<PlainTextDataBlock> m_plainTextDataList;

            public struct PlainTextDataBlock {
                // Block Size
                public byte m_blockSize;
                // Plain Text Data
                public byte[] m_plainTextData;
            }
        }

        /// <summary>
        /// Application Extension
        /// </summary>
        private struct ApplicationExtension {
            // Extension Introducer
            public byte m_extensionIntroducer;
            // Extension Label
            public byte m_extensionLabel;
            // Block Size
            public byte m_blockSize;
            // Application Identifier
            public byte m_appId1, m_appId2, m_appId3, m_appId4, m_appId5, m_appId6, m_appId7, m_appId8;
            // Application Authentication Code
            public byte m_appAuthCode1, m_appAuthCode2, m_appAuthCode3;
            // Block Size & Application Data List
            public List<ApplicationDataBlock> m_appDataList;

            public struct ApplicationDataBlock {
                // Block Size
                public byte m_blockSize;
                // Application Data
                public byte[] m_applicationData;
            }

            public readonly string ApplicationIdentifier
            {
                get
                {
                    char[] c = { (char)this.m_appId1, (char)this.m_appId2, (char)this.m_appId3, (char)this.m_appId4, (char)this.m_appId5, (char)this.m_appId6, (char)this.m_appId7, (char)this.m_appId8 };
                    return new string(c);
                }
            }

            public readonly string ApplicationAuthenticationCode
            {
                get
                {
                    char[] c = { (char)this.m_appAuthCode1, (char)this.m_appAuthCode2, (char)this.m_appAuthCode3 };
                    return new string(c);
                }
            }

            public readonly int LoopCount
            {
                get
                {
                    if (this.m_appDataList == null || this.m_appDataList.Count < 1 ||
						this.m_appDataList[0].m_applicationData.Length < 3 ||
						this.m_appDataList[0].m_applicationData[0] != 0x01) {
                        return 0;
                    }
                    return BitConverter.ToUInt16(this.m_appDataList[0].m_applicationData, 1);
                }
            }
        }
    }
}