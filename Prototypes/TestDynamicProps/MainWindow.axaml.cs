using Avalonia.Controls;
using System.Collections.Generic;
using System.Dynamic;

namespace TestDynamicProps
{
    public partial class MainWindow : Window
    {
        private Sadeh _sadeh = new Sadeh();

        const string Cell1Name = "MyObj";
        const string Cell2Name = "MyObj2";


        const string Text1 = "Hello World!";
        const string Text2 = "Hi World!";



        const string Text3 = "This is me";
        const string Text4 = "This is me saying hey";

        public MainWindow()
        {
            _sadeh.SetCell(Cell1Name, typeof(string), true);

            _sadeh.SetCell(Cell2Name, typeof(string), true);

            ToggleText();
            ToggleText2();

            InitializeComponent();

            RootPanel.DataContext = _sadeh;
        }

        private void ToggleTextImpl(string cellName, string text1, string text2)
        {
            if (_sadeh[cellName] == text1)
            {
                _sadeh[cellName] = text2;
            }
            else
            {
                _sadeh[cellName] = text1;
            }
        }

        public void ToggleText()
        {
            ToggleTextImpl(Cell1Name, Text1, Text2);
        }

        public void ToggleText2()
        {
            ToggleTextImpl(Cell2Name, Text3, Text4);
        }
    }
}
