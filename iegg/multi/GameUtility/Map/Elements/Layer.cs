using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using multi.GameUtility.Map.Elements.SheetInfo;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace multi.GameUtility.Map.Elements
{
    public class Layer : ILayer
    {
        public VertexPosTexNormal[] TileElements { get; set; }
        public Sheet TileSheetData { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string SheetName { get; set; }

        private void LoadTileMap(string SheetName)
        {
            StreamReader reader = new StreamReader($"Content\\MapElements\\{SheetName}.json");
            TileSheetData = JsonConvert.DeserializeObject<Sheet>(reader.ReadToEnd());
        }

        public Layer(int w, int h, float z, int r, string sheetName, ContentManager content)
        {
            this.W = w;
            this.H = h;

            SheetName = (SheetName == null) ? sheetName : SheetName;
            TileElements = new VertexPosTexNormal[w * h * 6];
            LoadTileMap((SheetName!=null)?SheetName:sheetName);
            TileSheetData.Load(content);
            LoadMapData(w, h, z, r);
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            foreach (EffectPass pass in ef.CurrentTechnique.Passes)
            {
                ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name));
                ef.Parameters["xWorld"].SetValue(Matrix.Identity);

                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, TileElements.ToArray(), 0, TileElements.Count() / 3, VertexPosTexNormal.VertexDeclaration);
            }
        }

        public void LayerClick(float clickX, float clickY)
        {
            //  IEnumerable<VertexPosTexNormal> points2Change = TileElements.Where(p => p.);
            int log = 0;
            for (int i = 0; i < TileElements.Length; i++)
            {
                if (TileElements[i].position.X == clickX && TileElements[i].position.Z == clickY)
                {
                    log++;
                    VertexPosTexNormal temp = TileElements[i];
                    temp.SetTaken();
                    TileElements[i] = temp;
                }
            }

            RecalculateTitlePaths();
        }

        private void RecalculateTitlePaths()
        {
            for (int i = 0; i < TileElements.Length; i += 6)
            {
                VertexPosTexNormal[] quadElements = TileElements.Skip(i).Take(6).ToArray();
                int[] pattern = { quadElements[0].Taken, (quadElements[1].Taken ==0? quadElements[4].Taken: quadElements[1].Taken), (quadElements[2].Taken == 0 ? quadElements[3].Taken : quadElements[2].Taken), quadElements[5].Taken };
                TileElement stg1 = TileSheetData.TileElements.Where(x => x.Groups.Any(y => y.Pattern.SequenceEqual(pattern))).FirstOrDefault();
                if (stg1 != null)
                {
                    ElementsGroup texture2Put = stg1.Groups.Where(x => x.Pattern.SequenceEqual(pattern)).FirstOrDefault();
                    if (texture2Put != null)
                    {
                        int value;
                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                        {
                            byte[] randomNumber = new byte[4];//4 for int32
                            rng.GetBytes(randomNumber);
                            value = BitConverter.ToInt32(randomNumber, 0);
                        }

                        int indexRand = new Random(value).Next((texture2Put.Positions.Count <= 1 ? 1 : texture2Put.Positions.Count));

                        float uvX = (float)texture2Put.Positions[indexRand].X / (ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width);
                        float uvY = (float)texture2Put.Positions[indexRand].Y / (ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height);

                        float uvX2 = ((float)texture2Put.Positions[indexRand].X / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width) + ((float)TileSheetData.TileElementWeight / ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width);
                        float uvY2 = ((float)texture2Put.Positions[indexRand].Y / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height) - ((float)TileSheetData.TileElementHeight / ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height);

                        VertexPosTexNormal temp = TileElements[i];
                        temp.texCoord = new Vector2(uvX, uvY);
                        TileElements[i] = temp;

                        temp = TileElements[i + 1];
                        temp.texCoord = new Vector2(uvX, uvY2);
                        TileElements[i + 1] = temp;

                        temp = TileElements[i + 2];
                        temp.texCoord = new Vector2(uvX2, uvY);
                        TileElements[i + 2] = temp;

                        temp = TileElements[i + 3];
                        temp.texCoord = new Vector2(uvX2, uvY);
                        TileElements[i + 3] = temp;

                        temp = TileElements[i + 4];
                        temp.texCoord = new Vector2(uvX, uvY2);
                        TileElements[i + 4] = temp;

                        temp = TileElements[i + 5];
                        temp.texCoord = new Vector2(uvX2, uvY2);
                        TileElements[i + 5] = temp;
                    }
                }
            }
        }

        public void LoadMapData(int terrainWidth, int terrainHeight, float terrainZ, int r)
        {
            ElementsGroup element = TileSheetData.TileElements.Where(x => x.Groups.Any(z => z.Name == "Default")).FirstOrDefault().Groups.Where(y => y.Name == "Default").FirstOrDefault();
            int TableIterator = 0;
            for (int x = 0; x < terrainWidth; x += 1)
            {
                for (int y = 0; y < terrainHeight; y += 1)
                {
                    int value;
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        byte[] randomNumber = new byte[4];//4 for int32
                        rng.GetBytes(randomNumber);
                        value = BitConverter.ToInt32(randomNumber, 0);
                    }

                    int indexRand = new Random(value).Next(element.Positions.Count);

                    float uvX = (float)element.Positions[indexRand].X / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width;
                    float uvY = (float)element.Positions[indexRand].Y / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height;

                    float uvX2 = ((float)element.Positions[indexRand].X / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width) + ((float)TileSheetData.TileElementWeight / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Width);
                    float uvY2 = ((float)element.Positions[indexRand].Y / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height) + ((float)TileSheetData.TileElementHeight / (float)ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name).Height);

                    VertexPosTexNormal _temp = new VertexPosTexNormal(new Vector3(x, terrainZ, y) * r, new Vector2(uvX, uvY), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;

                    _temp = new VertexPosTexNormal(new Vector3(x, terrainZ, y + 1) * r, new Vector2(uvX, uvY2), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;

                    _temp = new VertexPosTexNormal(new Vector3(x + 1, terrainZ, y) * r, new Vector2(uvX2, uvY), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;


                    _temp = new VertexPosTexNormal(new Vector3(x + 1, terrainZ, y) * r, new Vector2(uvX2, uvY), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;


                    _temp = new VertexPosTexNormal(new Vector3(x, terrainZ, y + 1) * r, new Vector2(uvX, uvY2), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;

                    _temp = new VertexPosTexNormal(new Vector3(x + 1, terrainZ, y + 1) * r, new Vector2(uvX2, uvY2), new Vector3(0, 1, 0));
                    TileElements[TableIterator] = (_temp);
                    TableIterator++;
                }
            }
        }

        public void LoadTitle(string sheet)
        {

        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
