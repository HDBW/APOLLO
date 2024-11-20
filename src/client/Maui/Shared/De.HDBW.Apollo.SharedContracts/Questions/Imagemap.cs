// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Imagemap : AbstractQuestion, ICalculateScore<string>
    {
        public Imagemap(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, modulId, assessmentId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            Shapes = new List<Shape>()
            {
                CreateShape(data.form1, data.coordinates1),
                CreateShape(data.form2, data.coordinates2),
                CreateShape(data.form3, data.coordinates3),
                CreateShape(data.form4, data.coordinates4),
            };
            var option1 = new List<string> { data.m_imagemap_option1_1, data.m_imagemap_option1_2, data.m_imagemap_option1_3, data.m_imagemap_option1_4 };
            var option2 = new List<string> { data.m_imagemap_option2_1, data.m_imagemap_option2_2, data.m_imagemap_option2_3, data.m_imagemap_option2_4 };
            var option3 = new List<string> { data.m_imagemap_option3_1, data.m_imagemap_option3_2, data.m_imagemap_option3_3, data.m_imagemap_option3_4 };
            var option4 = new List<string> { data.m_imagemap_option4_1, data.m_imagemap_option4_2, data.m_imagemap_option4_3, data.m_imagemap_option4_4 };
            CreateAditionalData(1, option1, data.m_imagemap_credit1);
            CreateAditionalData(2, option2, data.m_imagemap_credit2);
            CreateAditionalData(3, option3, data.m_imagemap_credit3);
            CreateAditionalData(4, option4, data.m_imagemap_credit4);
        }

        public Image? Image
        {
            get { return Data.image; }
        }

        public Dictionary<int, string> Scores { get; } = new Dictionary<int, string>();

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public IEnumerable<Shape> Shapes { get; }

        public int NumberOfChoices { get; set; }

        public double? CalculateScore(string selection)
        {
            var score = Scores.FirstOrDefault(x => x.Value == selection);
            return Credits.TryGetValue(score.Key, out double value) ? value : null;
        }

        private void CreateAditionalData(int index, List<string> options, string credit)
        {
            options = options.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (options.Any())
            {
                var pattern = string.Join(";", options);
                Scores.Add(index, pattern);
                Credits.Add(index, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
            }
        }

        private Shape CreateShape(ShapeType form, string coordinates)
        {
            switch (form)
            {
                case ShapeType.Circle:
                    return new CircleShape(coordinates);
                case ShapeType.Rectangle:
                    return new RectangleShape(coordinates);
                case ShapeType.Polygon:
                    return new PolygonShape(coordinates);
                default:
                    throw new NotSupportedException($"ShapeType {form} not supported.");
            }
        }
    }
}
