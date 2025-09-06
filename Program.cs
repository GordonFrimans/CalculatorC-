using System;
using System.Globalization;

namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Простой калькулятор ===");
            Console.WriteLine("Поддерживаемые операции:");
            Console.WriteLine("Бинарные: +, -, *, /, % (остаток от деления)");
            Console.WriteLine("Унарные: 1/x (обратное число), x^2 (квадрат)");
            Console.WriteLine("Для выхода введите 'q' или 'quit'\n");

            while (true)
            {
                try
                {
                    // Ввод первого числа
                    Console.Write("Введите первое число: ");
                    string input1 = Console.ReadLine();
                    if (IsExitCommand(input1))
                        break;

                    if (!double.TryParse(input1, NumberStyles.Float, CultureInfo.InvariantCulture, out double num1))
                    {
                        Console.WriteLine("Ошибка: Некорректный формат первого числа\n");
                        continue;
                    }

                    // Ввод операции
                    Console.Write("Введите операцию (+, -, *, /, %, 1/x, x^2): ");
                    string operation = Console.ReadLine()?.Trim();
                    if (IsExitCommand(operation))
                        break;

                    if (string.IsNullOrEmpty(operation) || !IsValidOperation(operation))
                    {
                        Console.WriteLine("Ошибка: Неподдерживаемая операция\n");
                        continue;
                    }

                    // Проверяем, является ли операция унарной
                    if (IsUnaryOperation(operation))
                    {
                        // Для унарных операций второе число не требуется
                        double result = PerformUnaryCalculation(num1, operation);
                        Console.WriteLine($"Результат: {operation}({num1}) = {result:G15}");
                    }
                    else
                    {
                        // Ввод второго числа для бинарных операций
                        Console.Write("Введите второе число: ");
                        string input2 = Console.ReadLine();
                        if (IsExitCommand(input2))
                            break;

                        if (!double.TryParse(input2, NumberStyles.Float, CultureInfo.InvariantCulture, out double num2))
                        {
                            Console.WriteLine("Ошибка: Некорректный формат второго числа\n");
                            continue;
                        }

                        // Выполнение бинарного вычисления
                        double result = PerformBinaryCalculation(num1, num2, operation);
                        Console.WriteLine($"Результат: {num1} {operation} {num2} = {result:G15}");
                    }

                    Console.WriteLine(new string('-', 40));
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Ошибка: Деление на ноль недопустимо\n");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка операции: {ex.Message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}\n");
                }
            }

            Console.WriteLine("До свидания!");
        }

        static bool IsExitCommand(string input)
        {
            return string.Equals(input?.Trim(), "q", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input?.Trim(), "quit", StringComparison.OrdinalIgnoreCase);
        }

        static bool IsValidOperation(string operation)
        {
            return operation == "+" || operation == "-" || operation == "*" || operation == "/" ||
            operation == "%" || operation == "1/x" || operation == "x^2";
        }

        static bool IsUnaryOperation(string operation)
        {
            return operation == "1/x" || operation == "x^2";
        }

        static double PerformBinaryCalculation(double num1, double num2, string operation)
        {
            return operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" when num2 != 0 => num1 / num2,
                "/" when num2 == 0 => throw new DivideByZeroException(),
                "%" when num2 != 0 => num1 % num2,
                "%" when num2 == 0 => throw new DivideByZeroException(),
                _ => throw new ArgumentException("Неподдерживаемая бинарная операция")
            };
        }

        static double PerformUnaryCalculation(double num, string operation)
        {
            return operation switch
            {
                "1/x" when num != 0 => 1.0 / num,
                "1/x" when num == 0 => throw new DivideByZeroException(),
                "x^2" => num * num,
                _ => throw new ArgumentException("Неподдерживаемая унарная операция")
            };
        }
    }
}
