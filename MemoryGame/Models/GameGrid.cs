using MemoryGame.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MemoryGame.Models
{
    public class GameGrid
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string PictureSetFilePath { get; set; }
        public List<string> Images { get; set; }
        public List<List<GridCell>> GridCells { get; set; }

        public GameGrid() { }

        public GameGrid(int rows, int columns, PictureSet pictureSet)
        {
            Rows = rows;
            Columns = columns;
            Images = new List<string>();
            GridCells = new List<List<GridCell>>();

            LoadPictureSetPath(pictureSet);
            LoadAndRandomizeImages();
            GenerateGrid();
        }

        private void LoadPictureSetPath(PictureSet pictureSet)
        {
            PictureSetFilePath = pictureSet switch
            {
                PictureSet.Animals => "Data/Animals",
                PictureSet.Cars => "Data/Cars",
                PictureSet.Food => "Data/Food",
                _ => "Data/Default"
            };
        }

        private void LoadAndRandomizeImages()
        {
            for (int i = 0; i < 18; i++)
            {
                Images.Add($"{PictureSetFilePath}/image{i + 1}.jpg");
            }

            Random rng = new Random();
            int n = Images.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = Images[k];
                Images[k] = Images[n];
                Images[n] = value;
            }
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < Rows; row++)
            {
                var rowList = new List<GridCell>();
                for (int col = 0; col < Columns; col++)
                {
                    rowList.Add(new GridCell());
                }
                GridCells.Add(rowList);
            }

            int nImages = Rows * Columns / 2;
            Random rng = new Random();

            for (int i = 0; i < nImages; i++)
            {
                AssignImageToRandomCell(Images[i], rng);
                AssignImageToRandomCell(Images[i], rng);
            }
        }

        private void AssignImageToRandomCell(string imagePath, Random rng)
        {
            while (true)
            {
                int randomRow = rng.Next(Rows);
                int randomCol = rng.Next(Columns);

                if (!GridCells[randomRow][randomCol].HasPictureAssigned)
                {
                    GridCells[randomRow][randomCol].PictureFilePath = imagePath;
                    break;
                }
            }
        }

        public IEnumerable<GridCell> Cells
        {
            get
            {
                foreach (var row in GridCells)
                {
                    foreach (var cell in row)
                    {
                        yield return cell;
                    }
                }
            }
        }

        public void SaveToFile(string folderPath)
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

            string saveName = "save_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
            string totalFilePath = Path.Combine(folderPath, saveName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(totalFilePath, json);
        }


        public static GameGrid LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameGrid>(json);
        }
    }
}