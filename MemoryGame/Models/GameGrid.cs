using MemoryGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MemoryGame.Models
{
    public class GameGrid
    {
        private int _rows;
        private int _columns;
        private string _pictureSetFilePath;
        private List<string> _images;
        private GridCell[,] gridCells;

        public GameGrid(int rows, int columns, PictureSet pictureSet)
        {
            _rows = rows;
            _columns = columns;
            _images = new List<string>();
            gridCells = new GridCell[rows, columns];

            LoadPictureSetPath(pictureSet);
            LoadAndRandomizeImages();
            GenerateGrid();
        }

        private void LoadPictureSetPath(PictureSet pictureSet)
        {
            switch(pictureSet)
            {
                case PictureSet.Animals:
                    {
                        _pictureSetFilePath = "Data/Animals";
                        break;
                    }
                case PictureSet.Cars:
                    {
                        _pictureSetFilePath = "Data/Cars";
                        break;
                    }
                case PictureSet.Food:
                    {
                        _pictureSetFilePath = "Data/Food";
                        break;
                    }
            }
        }

        private void LoadAndRandomizeImages()
        {
            for (int i = 0; i < 18; i++)
            {
                _images.Add($"{_pictureSetFilePath}/image{i + 1}.jpg");
            }

            Random rng = new Random();
            int n = _images.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                String value = _images[k];
                _images[k] = _images[n];
                _images[n] = value;
            }
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    gridCells[row, col] = new GridCell();
                }
            }

            int nImages = _rows * _columns / 2;

            for (int i = 0; i < nImages; i++)
            {
                Random rng = new Random();

                while (true)
                {
                    int randomRow = rng.Next(_rows);
                    int randomColumn = rng.Next(_columns);

                    if (!gridCells[randomRow, randomColumn].HasPictureAssigned)
                    {
                        gridCells[randomRow, randomColumn].PictureFilePath = _images[i];
                        break;
                    }
                }

                while (true)
                {
                    int randomRow = rng.Next(_rows);
                    int randomColumn = rng.Next(_columns);

                    if (!gridCells[randomRow, randomColumn].HasPictureAssigned)
                    {
                        gridCells[randomRow, randomColumn].PictureFilePath = _images[i];
                        break;
                    }
                }
            }
        }
        public IEnumerable<GridCell> Cells
        {
            get
            {
                foreach (var cell in gridCells)
                    yield return cell;
            }
        }
    }
}
