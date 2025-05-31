using System.Linq.Expressions;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WF_Proyecto002
{
    public partial class Form1 : Form
    {
        private string? base64Image;
        private string? mimeType;

        public Form1()
        {
            InitializeComponent();
        }

        public void Btn_SeleccionarFoto_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar foto";
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    base64Image = Convert.ToBase64String(File.ReadAllBytes(imagePath));

                    // 3. Detectar tipo MIME
                    mimeType = Path.GetExtension(imagePath).ToLower() switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".bmp" => "image/bmp",
                        ".gif" => "image/gif",
                        _ => "application/octet-stream"
                    };

                    pictureBox1.Image = Image.FromFile(imagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; //
                }
            }
        }

        public async void Btn_Enviar_Click(object sender, EventArgs e)
        {
            int totalCalorias = ObtenerTotalCalorias();
            string apiKey = "API KEY";
            string prompt = "Analiza la imagen proporcionada y responde con Comida: el nombre de la comida, Calorias: luego un numero que sea el aproximado de las calorias y luego una recomendacion saludable o indica si esa comida ya es saludable, se lo mas breve posible" +
                $"El total de calorías registradas es {totalCalorias}. ¿Debería bajar mi consumo de calorías? Dame una recomendación urgente y breve según ese total. Por ejemplo : Calorias muy altas reducir consumo urgentemente ó Calorias estables, sigue así, ";
            string respuestaJson = await EnviarImagenAGemini(apiKey, base64Image, prompt, mimeType);


            string textoRespuesta = ExtraerTextoGemini(respuestaJson);





            TBRespuesta.Text = textoRespuesta;


            var datos = ExtraerDatosComida(TBRespuesta.Text);
            if (datos.Comida != null && datos.Calorias != null)
            {
                GuardarEnBaseDeDatos(datos.Comida, datos.Calorias.Value, DateTime.Now);
            }
            else
            {
                MessageBox.Show("No se extrajeron datos válidos del texto del TextBox.");
            }

            static async Task<string> EnviarImagenAGemini(string apiKey, string base64Image, string prompt, string mimeType)
            {
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

                var body = new
                {
                    contents = new[]
                    {
                new {
                    parts = new object[]
                    {
                        new { text = prompt },
                        new {
                            inlineData = new {
                                mimeType = mimeType,
                                data = base64Image
                            }
                        }
                    }
                }
            }
                };

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    }
                }
            }

            static string ExtraerTextoGemini(string json)
            {
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    var text = root
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text ?? "No se encontró texto en la respuesta.";
                }
                catch
                {
                    return "No se pudo extraer el texto de la respuesta.";
                }
            }
            TBRespuesta.Text = textoRespuesta;

        }
        private (string? Comida, int? Calorias) ExtraerDatosComida(string texto)
        {
            try
            {
                // Busca "Comida: ..." y "Calorias: ..."
                var comidaMatch = System.Text.RegularExpressions.Regex.Match(texto, @"Comida:\s*(.+)");
                var caloriasMatch = System.Text.RegularExpressions.Regex.Match(texto, @"Calorias:\s*(\d+)");

                string? comida = comidaMatch.Success ? comidaMatch.Groups[1].Value.Trim() : null;
                int? calorias = caloriasMatch.Success ? int.Parse(caloriasMatch.Groups[1].Value) : null;

                return (comida, calorias);
            }
            catch
            {
                return (null, null);
            }
        }

        private void GuardarEnBaseDeDatos(string comida, int calorias, DateTime fecha)
        {
            try
            {
                string connectionString = "Server=USUARIO_PC\\SQLEXPRESS;Database=DB_ProyectoFinal002;Integrated Security=True; TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO REGISTRO_PF002 (Comida, Calorias, Fecha) VALUES (@comida, @calorias, @fecha)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@comida", comida);
                        command.Parameters.AddWithValue("@calorias", calorias);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Guardado correctamente. ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en la base de datos: " + ex.Message);
            }

        }
        private int ObtenerTotalCalorias()
        {
            int total = 0;
            string connectionString = "Server=USUARIO_PC\\SQLEXPRESS;Database=DB_ProyectoFinal002;Integrated Security=True; TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(Calorias) FROM REGISTRO_PF002";
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                        total = Convert.ToInt32(result);
                }
            }
            return total;
        }

        private void BTNReset_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Server=USUARIO_PC\\SQLEXPRESS;Database=DB_ProyectoFinal002;Integrated Security=True; TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Eliminar todos los registros
                    string deleteQuery = "DELETE FROM REGISTRO_PF002";
                    using (var deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Reiniciar el IDENTITY
                    string reseedQuery = "DBCC CHECKIDENT ('REGISTRO_PF002', RESEED, 0)";
                    using (var reseedCommand = new SqlCommand(reseedQuery, connection))
                    {
                        reseedCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Base de datos limpiada y el ID reiniciado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar la base de datos: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }

}



