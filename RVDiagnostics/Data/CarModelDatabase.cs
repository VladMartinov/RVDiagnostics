using RVDiagnostics.Models;

namespace RVDiagnostics.Data
{
    public static class CarModelDatabase
    {
        public static readonly Dictionary<string, CarModel> Models = new()
        {
            {
               "88GB-12A650-AB",
                new CarModel
                {
                    Brand = "Ford",
                    Model = "Scorpio",
                    Years = "1985-1992",
                    Engine = "2.0i DOHC (N9D)",
                    Ecu = "EEC-IV (88GB-12A650-AB)",
                    Protocol = "Ford OBD1 PWM",
                    Connector = "3-pin diagnostic port (STI/STO)",
                    Tests = new List<string>
                    {
                        "KOEO (Key On Engine Off) Self-Test",
                        "KOER (Key On Engine Running) Self-Test",
                        "Wiggle Test (Intermittent Faults)"
                    },
                    Description = "Ford Scorpio первого поколения с двигателем 2.0 DOHC (код N9D). ЭБУ EEC-IV поддерживает чтение кодов неисправностей через диагностический вывод STO (Self-Test Output). Тесты включают статическую (KOEO) и динамическую (KOER) проверки, а также тестирование методом тряски (Wiggle Test). Для подключения используется 3-контактный диагностический разъём в моторном отсеке.",
                    DescriptionAlt = "First generation Ford Scorpio with 2.0 DOHC engine (code N9D). The EEC-IV ECU supports reading fault codes via the STO diagnostic output (Self-Test Output). The tests include static (KOEO) and dynamic (KOER) checks, as well as the Wiggle Test. A 3-pin diagnostic connector in the engine compartment is used for connection.",
                    CarImagePath = "/Assets/Cars/Ford_Scorpio_1.jpg",
                    ConnectorImagePath = "/Assets/Connectors/EEC-IV_3pin.png"
                }
            }
        };
    }
}