using RestSharp;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;




namespace Koch_App
{
    public partial class MainPage : ContentPage
    {
        private List<string> lsIngredients;
        public List<string> LsIngredients
        {
            get => lsIngredients;
            set
            {
                lsIngredients = value;
                OnPropertyChanged(nameof(lsIngredients));
            }
        }

        public BindableObject IngredientsList { get; private set; }

        public MainPage()
        {
            LsIngredients = new();
            InitializeComponent();
        }



        public Microsoft.Maui.Controls.Image image = new()
        {
            Source = ImageSource.FromUri(new Uri("https://images.pexels.com/photos/3310691/pexels-photo-3310691.jpeg?_gl=1*12am39z*_ga*MTg2MzE2MTczOC4xNzczMDUwNzQx*_ga_8JE65Q40S6*czE3NzMwNTA3NDAkbzEkZzEkdDE3NzMwNTA5ODUkajU5JGwwJGgw"))
        };

        private void DropDown_Clicked(object sender, EventArgs e)
        {
            Liste1.IsVisible = !Liste1.IsVisible;
            if (TagList.IsVisible == true)
            {
                TagList.IsVisible = false;
                TagList1.IsVisible = false;
                TagList2.IsVisible = false;
                TagList3.IsVisible = false;
            }

            // wenn ein filter checked ist dann mach manche weg die nicht gehen mit dem filter
        }
        private void Tag_Clicked(object sender, EventArgs e)
        {
            TagList.IsVisible = !TagList.IsVisible;
            TagList1.IsVisible = !TagList1.IsVisible;
            TagList2.IsVisible = !TagList2.IsVisible;
            TagList3.IsVisible = !TagList3.IsVisible;
            if (Liste1.IsVisible == true) ;
            {
                Liste1.IsVisible = false;
            }
        }
        async Task<Meals> GetMealsByName(string text)
        {
            var client = new RestClient("https://www.themealdb.com/api/json/v1/1/search.php?s=" + text);
            var response = await client.GetAsync<Meals>(new RestRequest());

            return response;

        }

        private void entry_Completed(object sender, EventArgs e) // wenn  das erste mal Enter gedrückt wird macht es das was hier ist
        {
          Ingredients_Teiler.IsVisible = true;
            InstructionsText.IsVisible = true;
            string text = ((Entry)sender).Text; // wartet bis man das zweite mal enter gedrückt hat und kuckt denn text an. ka wie es anders geht

            if (text != null)
            {
                if (Name.IsChecked == true)
                {
                    Task.Run(async () =>
                    {
                        var data = await GetMealsByName(text);

                        CountTexts.Text = ("Count: " + (data.LsMeals != null ? data.LsMeals?.Count : "0"));
                        foreach (var meal in data.LsMeals ?? [])
                        {

                            string MealNameText = "Name: " + meal.Gericht;

                            var propVegan = meal.Vegane_Variante;
                            if (!string.IsNullOrWhiteSpace(propVegan))
                            {
                                string VegenAlt_Mainpage = ("Vegan Alternative? " + meal.Vegane_Variante);
                            }

                            var propTag = meal.Tag;
                            if (propTag != null)
                            {
                                string Tag_Mainpage = ("Tags: " + meal.Tag);
                            }

                            var propHerkunft = meal.Herkunft;
                            if (!string.IsNullOrWhiteSpace(propHerkunft))
                            {
                                string Origins_Mainpage = ("Origin:" + meal.Herkunft);
                            }

                            var propBeschreibung = meal.Beschreibung;
                            if (!string.IsNullOrWhiteSpace(propBeschreibung))
                            {
                                string Description_Mainpage = ("Description: " + meal.Beschreibung);
                            }

                            var lsIngredients = new List<string>();

                            foreach (var property in meal.GetType().GetProperties())
                            {
                                var propName = property.Name;
                                var propValue = meal.GetType().GetProperty(propName)?.GetValue(meal)?.ToString();

                                if (propName.Contains("Zutat") && !string.IsNullOrWhiteSpace(propValue))
                                {

                                    if (int.TryParse(propName.Replace("Zutat", string.Empty), out int index))
                                    {
                                        var quantity = meal.GetType().GetProperty("Anzahl" + index)?.GetValue(meal)?.ToString();
                                        var IngredientsNames = quantity + " " + propValue + "\n";
                                        lsIngredients.Add(IngredientsNames);
                                        LsIngredients.Add(IngredientsNames);
                                    }
                                }
                            }

                            BindableLayout.SetItemsSource(IngredientsColltector, lsIngredients);
                        
                            var propAnleitung = meal.Anleitung;
                            if (!string.IsNullOrWhiteSpace(propAnleitung))
                            {
                                string Guide_Mainpage = (meal.Anleitung + "\n");
                            }
                            
                        }
                    });
                }
            }
        }
    }
}
public class Meals
{
    [JsonPropertyName("meals")]
    public List<Meal> ?LsMeals { get; set; }
}
public class Meal
{
    [JsonPropertyName("idMeal")]
    public string? Id { get; set; }
    [JsonPropertyName("strMeal")]
    public string? Gericht { get; set; }
    [JsonPropertyName("strMealAlternate")]
    public string? Vegane_Variante { get; set; }
    [JsonPropertyName("strCategory")]
    public string? Beschreibung { get; set; }
    [JsonPropertyName("strArea")]
    public string? Herkunft { get; set; }
    [JsonPropertyName("strInstructions")]
    public string? Anleitung { get; set; }
    [JsonPropertyName("strTags")]
    public string? Tag { get; set; }
    [JsonPropertyName("strIngredient1")]
    public string? Zutat1 { get; set; }
    [JsonPropertyName("strIngredient2")]
    public string? Zutat2 { get; set; }
    [JsonPropertyName("strIngredient3")]
    public string? Zutat3 { get; set; }
    [JsonPropertyName("strIngredient4")]
    public string? Zutat4 { get; set; }
    [JsonPropertyName("strIngredient5")]
    public string? Zutat5 { get; set; }
    [JsonPropertyName("strIngredient6")]
    public string? Zutat6 { get; set; }
    [JsonPropertyName("strIngredient7")]
    public string? Zutat7 { get; set; }
    [JsonPropertyName("strIngredient8")]
    public string? Zutat8 { get; set; }
    [JsonPropertyName("strIngredient9")]
    public string? Zutat9 { get; set; }
    [JsonPropertyName("strIngredient10")]
    public string? Zutat10 { get; set; }
    [JsonPropertyName("strIngredient11")]
    public string? Zutat11 { get; set; }
    [JsonPropertyName("strIngredient12")]
    public string? Zutat12 { get; set; }
    [JsonPropertyName("strIngredient13")]
    public string? Zutat13 { get; set; }
    [JsonPropertyName("strIngredient14")]
    public string? Zutat14 { get; set; }
    [JsonPropertyName("strIngredient15")]
    public string? Zutat15 { get; set; }
    [JsonPropertyName("strIngredient16")]
    public string? Zutat16 { get; set; }
    [JsonPropertyName("strIngredient17")]
    public string? Zutat17 { get; set; }
    [JsonPropertyName("strIngredient18")]
    public string? Zutat18 { get; set; }
    [JsonPropertyName("strIngredient19")]
    public string? Zutat19 { get; set; }
    [JsonPropertyName("strIngredient20")]
    public string? Zutat20 { get; set; }
    [JsonPropertyName("strMeasure1")]
    public string? Anzahl1 { get; set; }
    [JsonPropertyName("strMeasure2")]
    public string? Anzahl2 { get; set; }
    [JsonPropertyName("strMeasure3")]
    public string? Anzahl3 { get; set; }
    [JsonPropertyName("strMeasure4")]
    public string? Anzahl4 { get; set; }
    [JsonPropertyName("strMeasure5")]
    public string? Anzahl5 { get; set; }
    [JsonPropertyName("strMeasure6")]
    public string? Anzahl6 { get; set; }
    [JsonPropertyName("strMeasure7")]
    public string? Anzahl7 { get; set; }
    [JsonPropertyName("strMeasure8")]
    public string? Anzahl8 { get; set; }
    [JsonPropertyName("strMeasure9")]
    public string? Anzahl9 { get; set; }
    [JsonPropertyName("strMeasure10")]
    public string? Anzahl10 { get; set; }
    [JsonPropertyName("strMeasure11")]
    public string? Anzahl11 { get; set; }
    [JsonPropertyName("strMeasure12")]
    public string? Anzahl12 { get; set; }
    [JsonPropertyName("strMeasure13")]
    public string? Anzahl13 { get; set; }
    [JsonPropertyName("strMeasure14")]
    public string? Anzahl14 { get; set; }
    [JsonPropertyName("strMeasure15")]
    public string? Anzahl15 { get; set; }
    [JsonPropertyName("strMeasure16")]
    public string? Anzahl16 { get; set; }
    [JsonPropertyName("strMeasure17")]
    public string? Anzahl17 { get; set; }
    [JsonPropertyName("strMeasure18")]
    public string? Anzahl18 { get; set; }
    [JsonPropertyName("strMeasure19")]
    public string? Anzahl19 { get; set; }
    [JsonPropertyName("strMeasure20")]
    public string? Anzahl20 { get; set; }
    [JsonPropertyName("strSource")]
    public string? Quelle { get; set; }
    [JsonPropertyName("dateModified")]
    public string? Datum { get; set; }
}





