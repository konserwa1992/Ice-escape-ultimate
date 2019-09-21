using multi.GameUtility.Map.Elements;
using multi.GameUtility.Menu.Editor.Windows.Special;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Menu.Editor.Windows
{
    class AddNewMapElementWindow :Window
    {
        Grid gNewElement = new Grid();

        ComboBox cElementTypeList = new ComboBox()
        {
            Id = "cElementTypeList",
            Width = 200
        };

        public AddNewMapElementWindow(ObjectInspector inspector)
        {
            Width = 200;
            Height = 500;

            cElementTypeList.Items.Add(new ListItem() { Text = "Gun" });
            cElementTypeList.Items.Add(new ListItem() { Text = "Spawn" });
            cElementTypeList.Items.Add(new ListItem() { Text = "Normal Ice" });
            cElementTypeList.Items.Add(new ListItem() { Text = "Reverse Ice" });


            TextField tElementName = new TextField()
            {
                Id = "tElementName",
                Top = 30,
                Width = 200
            };

            Button bSpawnObject = new Button()
            {
                Top = 60,
                Width = 200,
                Text = "Spawn"
            };

            bSpawnObject.Click += new EventHandler(delegate (object sender, EventArgs args)
                {
                    Map.StandardElements objType = Map.StandardElements.Ignore;
                    switch (cElementTypeList.SelectedItem.Text)
                    {
                        case "Gun":
                            {
                                objType = Map.StandardElements.Gun;
                                break;
                            }
                        case "Spawn":
                            {
                                objType = Map.StandardElements.SpawnPiont;
                                break;
                            }
                        case "Normal Ice":
                        {
                            objType = Map.StandardElements.NormalIce;
                            break;
                        }
                        case "Reverse Ice":
                        {
                            objType = Map.StandardElements.ReverseIce;
                            break;
                        }

                    }

                    if (objType != Map.StandardElements.Ignore)
                    {
                        List<IMapElement> mapElementRefference = ((multi.Editor)MyraEnvironment.Game).Map.MapElements;
                        string result = ((multi.Editor)MyraEnvironment.Game).Map.AddElementOnMap(objType, tElementName.Text);
                        if (result != tElementName.Text)
                        {
                            var messageBox = Dialog.CreateMessageBox("Error", result);
                            messageBox.ShowModal(inspector.Desktop);
                        }
                    }
                    /*GunInspector paramInspector = new GunInspector(tElementName.Text);
                    Desktop.Widgets.Add(paramInspector);*/
                    this.Close();
                });



            gNewElement.Widgets.Add(cElementTypeList);
            gNewElement.Widgets.Add(tElementName);
            gNewElement.Widgets.Add(bSpawnObject);
            this.Content = gNewElement;
        }
    }
}
