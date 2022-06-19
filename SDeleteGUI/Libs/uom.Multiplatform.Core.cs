using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uom.Extensions;

#nullable enable

//	Using tuples to swap values
//	(li.BackColor, li.ForeColor) = (li.ForeColor, li.BackColor);

#region NULLables
/*
 
	https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/attributes/nullable-analysis 
	
System.Diagnostics.CodeAnalysis.AllowNullAttribute

[return: MaybeNull]
public T Find<T>(IEnumerable<T> sequence, Func<T, bool> predicate)

public static void ThrowWhenNull([NotNull] object? value, string valueExpression = "")
{
   _ = value ?? throw new ArgumentNullException(nameof(value), valueExpression);
   other logic elided
}

#nullable disable warnings
See All # Constants: https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/preprocessor-directives
*/
#endregion


#region NET Versions
//https://docs.microsoft.com/ru-ru/dotnet/standard/frameworks

#region .NET Framework
//NETFRAMEWORK, NET48, NET472, NET471, NET47, NET462, NET461, NET46, NET452, NET451, NET45, NET40, NET35, NET20
//NET48_OR_GREATER, NET472_OR_GREATER, NET471_OR_GREATER, NET47_OR_GREATER, NET462_OR_GREATER, NET461_OR_GREATER, NET46_OR_GREATER, NET452_OR_GREATER, NET451_OR_GREATER, NET45_OR_GREATER, NET40_OR_GREATER, NET35_OR_GREATER, NET20_OR_GREATER
#endregion

#region .NET Standard
//NETSTANDARD, NETSTANDARD2_1, NETSTANDARD2_0, NETSTANDARD1_6, NETSTANDARD1_5, NETSTANDARD1_4, NETSTANDARD1_3, NETSTANDARD1_2, NETSTANDARD1_1, NETSTANDARD1_0
//NETSTANDARD2_1_OR_GREATER, NETSTANDARD2_0_OR_GREATER, NETSTANDARD1_6_OR_GREATER, NETSTANDARD1_5_OR_GREATER, NETSTANDARD1_4_OR_GREATER, NETSTANDARD1_3_OR_GREATER, NETSTANDARD1_2_OR_GREATER, NETSTANDARD1_1_OR_GREATER, NETSTANDARD1_0_OR_GREATER
#endregion

#region .NET 5+ (и .NET Core)
//NET, NET6_0, NET6_0_ANDROID, NET6_0_IOS, NET6_0_MACOS, NET6_0_MACCATALYST, NET6_0_TVOS, NET6_0_WINDOWS, NET5_0, NETCOREAPP, NETCOREAPP3_1, NETCOREAPP3_0, NETCOREAPP2_2, NETCOREAPP2_1, NETCOREAPP2_0, NETCOREAPP1_1, NETCOREAPP1_0
//NET6_0_OR_GREATER, NET6_0_ANDROID_OR_GREATER, NET6_0_IOS_OR_GREATER, NET6_0_MACOS_OR_GREATER, NET6_0_MACCATALYST_OR_GREATER, NET6_0_TVOS_OR_GREATER, NET6_0_WINDOWS_OR_GREATER, NET5_0_OR_GREATER, NETCOREAPP_OR_GREATER, NETCOREAPP3_1_OR_GREATER, NETCOREAPP3_0_OR_GREATER, NETCOREAPP2_2_OR_GREATER, NETCOREAPP2_1_OR_GREATER, NETCOREAPP2_0_OR_GREATER, NETCOREAPP1_1_OR_GREATER, NETCOREAPP1_0_OR_GREATER
#endregion

#region Versions samples

#if NET6_0_WINDOWS || NET5_0_OR_GREATER || NET6_0_OR_GREATER || NET6_0_ANDROID || NET6_0_MACOS || NET6_0_IOS
#endif

#if NET48_OR_GREATER
#endif

#endregion

#endregion

//if (e is MethodCallExpression { Method.Name: "MethodName" })
#region Object Comparing, inline type conversions, Template comparsions https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/patterns#relational-patterns
/*	


	if (input is not (float or double))
	{
		return;
	}


	public override bool Equals(object? obj)	
		=> obj is LocalMemory memory && EqualityComparer<IntPtr>.Default.Equals(handle, memory.handle);


	static bool IsLetter(char c) 
		=> c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');

	static bool IsConferenceDay(DateTime date) 
		=> date is { Year: 2020, Month: 5, Day: 19 or 20 or 21 };

	static string GetCalendarSeason(DateTime date) 
		=> date.Month switch
	{
		3 or 4 or 5 => "spring",
		6 or 7 or 8 => "summer",
		9 or 10 or 11 => "autumn",
		12 or 1 or 2 => "winter",
		_ => throw new ArgumentOutOfRangeException(nameof(date), $"Date with unexpected month: {date.Month}."),
	};

	static string Classify(double measurement) 
		=> measurement switch
	{
		< -40.0 => "Too low",
		>= -40.0 and < 0 => "Low",
		>= 0 and < 10.0 => "Acceptable",
		>= 10.0 and < 20.0 => "High",
		>= 20.0 => "Too high",
		double.NaN => "Unknown",
	};

	static string Classify(Point point) => point switch
	{
		(0, 0) => "Origin",
		(1, 0) => "positive X basis end",
		(0, 1) => "positive Y basis end",
		_ => "Just a point",
	};


	static decimal GetGroupTicketPriceDiscount(int groupSize, DateTime visitDate)
		=> (groupSize, visitDate.DayOfWeek) switch
		{
			(<= 0, _) => throw new ArgumentException("Group size must be positive."),
			(_, DayOfWeek.Saturday or DayOfWeek.Sunday) => 0.0m,
			(>= 5 and < 10, DayOfWeek.Monday) => 20.0m,
			(>= 10, DayOfWeek.Monday) => 30.0m,
			(>= 5 and < 10, _) => 12.0m,
			(>= 10, _) => 15.0m,
			_ => 0.0m,
		};




		var numbers = new List<int> { 1, 2, 3 };
		if (SumAndCount(numbers) is (Sum: var sum, Count: > 0))
		{
			Console.WriteLine($"Sum of [{string.Join(" ", numbers)}] is {sum}");  // output: Sum of [1 2 3] is 6
		}

		static (double Sum, int Count) SumAndCount(IEnumerable<int> numbers)
		{
			int sum = 0;
			int count = 0;
			foreach (int number in numbers)
			{
				sum += number;
				count++;
			}
			return (sum, count);
		}




		public record Point2D(int X, int Y);
		public record Point3D(int X, int Y, int Z);
		static string PrintIfAllCoordinatesArePositive(object point) => point switch
		{
			Point2D (> 0, > 0) p => p.ToString(),
			Point3D (> 0, > 0, > 0) p => p.ToString(),
			_ => string.Empty,
		};

		В предыдущем примере используются позиционные записи, которые неявно обеспечивают выполнение метода Deconstruct.
		Используйте шаблон свойства в позиционном шаблоне, как показано в следующем примере:

		public record WeightedPoint(int X, int Y)
		{
			public double Weight { get; set; }
		}

		static bool IsInDomain(WeightedPoint point) => point is (>= 0, >= 0) { Weight: >= 0.0 };	

		Можно объединить два предыдущих варианта, как показано в следующем примере:
		if (input is WeightedPoint (> 0, > 0) { Weight: > 0.0 } p)
		{
			// ..
		}


		**************** Шаблон var
		Начиная с C# 7.0, шаблонvar можно использовать для сопоставления любого выражения, включая null, и присвоения результата сопоставления новой локальной переменной, как показано в следующем примере:

		static bool IsAcceptable(int id, int absLimit) =>
			SimulateDataFetch(id) is var results 
			&& results.Min() >= -absLimit 
			&& results.Max() <= absLimit;

		static int[] SimulateDataFetch(int id)
		{
			var rand = new Random();
			return Enumerable
					   .Range(start: 0, count: 5)
					   .Select(s => rand.Next(minValue: -10, maxValue: 11))
					   .ToArray();
		}


		Шаблон var полезно использовать, если в логическом выражении вам требуется временная переменная для хранения результатов промежуточных вычислений. Шаблон var также можно использовать, если требуется реализовать дополнительные проверки в условиях регистра when выражения или оператора switch, как показано в следующем примере:

		public record Point(int X, int Y);
		static Point Transform(Point point) => point switch
		{
			var (x, y) when x < y => new Point(-x, y),
			var (x, y) when x > y => new Point(x, -y),
			var (x, y) => new Point(x, y),
		};

		static void TestTransform()
		{
			Console.WriteLine(Transform(new Point(1, 2)));  // output: Point { X = -1, Y = 2 }
			Console.WriteLine(Transform(new Point(5, 2)));  // output: Point { X = 5, Y = -2 }
		}    
 */
#endregion


#region stackalloc vs fixed
/*		
	Выражение stackalloc выделяет блок памяти в стеке. Выделенный в стеке блок памяти, который создает этот метод,
	автоматически удаляется по завершении выполнения метода. 
	Вы не можете явным образом освободить память, выделенную stackalloc. 
	Выделенный в стеке блок памяти не подвергается сборке мусора, поэтому его не нужно закреплять с помощью инструкции fixed.

	Объем доступной памяти в стеке ограничен. 
	При выделении слишком большого объема памяти в стеке возникает исключение StackOverflowException. 
	Чтобы избежать этого, следуйте приведенным ниже правилам.

	Ограничьте объем памяти, выделенный stackalloc. 
	Например, если предполагаемый размер буфера меньше определенного предела, то выделяется память в стеке. 
	В противном случае используйте массив требуемой длины, как показано в следующем коде:

	const int MaxStackLimit = 1024;
	Span<byte> buffer = inputLength <= MaxStackLimit ? stackalloc byte[MaxStackLimit] : new byte[inputLength];

	!!! Мы рекомендуем везде, где это возможно, использовать для работы с выделенной в стеке памятью типы Span<T> или ReadOnlySpan<T>.!!!

	int length = 3;
	Span<int> numbers = stackalloc int[length];
	for (var i = 0; i < length; i++)
	{
		numbers[i] = i;
	}

	Старайтесь не использовать stackalloc в циклах. Выделяйте блок памяти за пределами цикла и используйте его повторно внутри цикла.


	Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
	var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
	Console.WriteLine(ind);  // output: 1


	unsafe
	{
		int length = 3;
		int* numbers = stackalloc int[length];
		for (var i = 0; i < length; i++)
		{
			numbers[i] = i;
		}
	}


	Содержимое только что выделенной памяти не определено. 
	Его следует инициализировать перед использованием. 
	Например, вы можете использовать метод Span<T>.Clear, который задает для всех элементов значение по умолчанию типа T.

	Начиная с версии C# 7.3, вы можете использовать синтаксис инициализатора массива, 
	чтобы определить содержимое для только что выделенной памяти. 
	В следующем примере показано несколько способов сделать это:

	Span<int> first = stackalloc int[3] { 1, 2, 3 };
	Span<int> second = stackalloc int[] { 1, 2, 3 };
	ReadOnlySpan<int> third = stackalloc[] { 1, 2, 3 };
	В выражении stackalloc T[E]T должен иметь неуправляемый тип, а E — неотрицательное значение int.

	Безопасность
	При использовании stackalloc в среде CLR автоматически включается контроль переполнения буфера. Если буфер переполнен, процесс незамедлительно прерывается — это позволяет минимизировать риск исполнения вредоносного кода.

*/
#endregion


#region with
//https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/with-expression

#endregion


#region Шаблоны списков c# 11
/*		

Начиная с C# 11, можно сопоставить массив или список с последовательностью шаблонов, соответствующих элементам. Можно применить любой из следующих шаблонов:

Любой шаблон можно применить к любому элементу, чтобы убедиться, что отдельный элемент соответствует определенным характеристикам.
Шаблон отмены (_) соответствует одному элементу.
Шаблон диапазона (..) может соответствовать нулю или нескольким элементам последовательности. В шаблоне списка допускается не более одного шаблона диапазона.
Шаблон var может захватывать один элемент или диапазон элементов.
Эти правила демонстрируются с помощью следующих объявлений массива:


int[] one = { 1 };
int[] odd = { 1, 3, 5 };
int[] even = { 2, 4, 6 };
int[] fib = { 1, 1, 2, 3, 5 };

Можно сопоставить всю последовательность, указав все элементы и используя значения:
Console.WriteLine(odd is [1, 3, 5]); // true
Console.WriteLine(even is [1, 3, 5]); // false (values)
Console.WriteLine(one is [1, 3, 5]); // false (length)

Некоторые элементы можно сопоставить в последовательности известной длины с помощью шаблона отмены (_) в качестве заполнителя:
Console.WriteLine(odd is [1, _, _]); // true
Console.WriteLine(odd is [_, 3, _]); // true
Console.WriteLine(even is [_, _, 5]); // false (last value)

Вы можете указать любое количество значений или заполнителей в любом месте последовательности. Если вы не обеспокоены длиной, можно использовать шаблон диапазона , чтобы соответствовать нулю или нескольким элементам:
Console.WriteLine(odd is [1, .., 3, _]); // true
Console.WriteLine(fib is [1, .., 3, _]); // true
Console.WriteLine(odd is [1, _, 5, ..]); // true
Console.WriteLine(fib is [1, _, 5, ..]); // false

В предыдущих примерах использовался шаблон константы , чтобы определить, является ли элемент заданным числом. Любой из этих шаблонов можно заменить другим шаблоном, например реляционным шаблоном:
Console.WriteLine(odd is [_, >1, ..]); // true
Console.WriteLine(even is [_, >1, ..]); // true
Console.WriteLine(fib is [_, > 1, ..]); // false

Шаблоны списков являются ценным инструментом, если данные не соответствуют обычной структуре. Сопоставление шаблонов можно использовать для проверки фигуры и значений данных вместо преобразования их в набор объектов.
Рассмотрим следующий фрагмент текстового файла, содержащего банковские транзакции:

Выходные данные
04-01-2020, DEPOSIT,    Initial deposit,            2250.00
04-15-2020, DEPOSIT,    Refund,                      125.65
04-18-2020, DEPOSIT,    Paycheck,                    825.65
04-22-2020, WITHDRAWAL, Debit,           Groceries,  255.73
05-01-2020, WITHDRAWAL, #1102,           Rent, apt, 2100.00
05-02-2020, INTEREST,                                  0.65
05-07-2020, WITHDRAWAL, Debit,           Movies,      12.57
04-15-2020, FEE,                                       5.55
Это формат CSV, но некоторые строки имеют больше столбцов, чем другие. Еще хуже для обработки один столбец в типе WITHDRAWAL содержит текст, созданный пользователем, и может содержать запятую в тексте. Шаблон списка, включающий шаблон отмены, шаблон констант и шаблон var для записи данных о значении в следующем формате:

decimal balance = 0m;
foreach (var record in ReadRecords())
{
	balance += record switch
	{
		[_, "DEPOSIT", _, var amount]     => decimal.Parse(amount),
		[_, "WITHDRAWAL", .., var amount] => -decimal.Parse(amount),
		[_, "INTEREST", var amount]       => decimal.Parse(amount),
		[_, "FEE", var fee]               => decimal.Parse(fee),
		_                                 => throw new InvalidOperationException($"Record {record} is not in the expected format!"),
	};
	Console.WriteLine($"Record: {record}, New balance: {balance:C}");
}
В предыдущем примере принимается строковый массив, где каждый элемент является одним полем в строке. Ключи выражения switch во втором поле, определяющее тип транзакции и количество оставшихся столбцов. Каждая строка гарантирует, что данные заданы в правильном формате. Шаблон отмены (_) пропускает первое поле с датой транзакции. Второе поле соответствует типу транзакции. Оставшиеся совпадения элементов пропускают поле с суммой. Последнее совпадение использует шаблон var для записи строкового представления суммы. Выражение вычисляет сумму для добавления или вычитания из баланса.
Шаблоны списка позволяют сопоставлять фигуру последовательности элементов данных. 
Вы используете шаблоны отмены и диапазона для сопоставления расположения элементов. 
Любой другой шаблон соответствует характеристикам отдельных элементов.

*/
#endregion


#region EventHandler
/*
	public event EventHandler<string> LineRead = delegate { };	
*/
#endregion


#region VOLATILE (Multithreading)
/*
	https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/volatile
	public volatile int sharedStorage;
*/
#endregion


#region YIELD - In IEnumerations RETURN
/*
	public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
	{
		int minIndex = str.IndexOf(searchstring);
		while (minIndex != -1)
		{
			yield return minIndex;
			minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
		}
	}
*/
#endregion


#region CATCH WHEN
/*
	var client = new HttpClient();
	var streamTask = client.GetStringAsync("https://localHost:10000");
	try
	{
		var responseText = await streamTask;
		return responseText;
	}
	catch (HttpRequestException e) when (e.Message.Contains("301"))
	{
		return "Site Moved";
	}
	catch (HttpRequestException e) when (e.Message.Contains("404"))
	{
		return "Page Not Found";
	}
	catch (HttpRequestException e)
	{
		return e.Message;
	} 
*/
#endregion


#region List Ranges And Indexes https://docs.microsoft.com/ru-ru/dotnet/csharp/whats-new/tutorials/ranges-indexes
/* 
	string[] words = new string[]
	{                // index from start    index from end
		"The",      // 0                   ^9
		"quick",    // 1                   ^8
		"brown",    // 2                   ^7
		"fox",      // 3                   ^6
		"jumped",   // 4                   ^5
		"over",     // 5                   ^4
		"the",      // 6                   ^3
		"lazy",     // 7                   ^2
		"dog"       // 8                   ^1
	};              // 9 (or words.Length) ^0
	Console.WriteLine($"The last word is {words[^1]}");
*/
#endregion


#region LINQ GroupBy Sample
/*
*** VB
Dim lExtList = (From FI In Me._lTotalFoundFilesOnDisk
	Group By FileExt = FI.Extension.ToLower Into Files = Group, Count()
	Order By FileExt).ToList


*** C#
var groupByLastNamesQuery =
	from student in students
	group student by student.LastName into newGroup
	orderby newGroup.Key
	select newGroup;

foreach (var nameGroup in groupByLastNamesQuery)
{
	Console.WriteLine($"Key: {nameGroup.Key}");
	foreach (var student in nameGroup)
	{
		Console.WriteLine($"\t{student.LastName}, {student.FirstName}");
	}
}
 */


//Also see .ToLookup() for any lists
//https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tolookup?view=netframework-4.8&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Linq.Enumerable.ToLookup%60%602);k(TargetFrameworkMoniker-.NETFramework,Version%253Dv4.8);k(DevLang-csharp)%26rd%3Dtrue
#endregion


#region RegEx Samples
/*
	private static readonly Regex SpacesPattern = new(@"\s");
	var v = SpacesPattern.Replace(value, match => string.Empty);

	private static readonly Regex ExponentPattern = new(@"[-+]?\d*\.?\d+[eE][-+]?\d+");
	if (ExponentPattern.IsMatch(v))    return new NumericParseResult(v.TryParseExponent());

	private static readonly Regex DecimalPattern = new(@"[\d\.\,\s]+");
	if (!DecimalPattern.IsMatch(value))    return null;//Doesn't look like a number at all.

	^\[(?<PropKey>.+)\]\:\s\[(?<PropValue>.+)(?:\r\n)*\]

	Private Shared ReadOnly _REO As System.Text.RegularExpressions.RegexOptions = C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline
	Private Shared ReadOnly _RegExp_SRPLogRow As String = My.Resources.SAFER_RegExp_SRPLogRow
	Private Shared ReadOnly _RegExp As New System.Text.RegularExpressions.Regex(_RegExp_SRPLogRow, _REO)

	Dim rMatch = _RegExp.Match(sLogFileRow)
	With rMatch
	If (rMatch.Success) Then
	Dim aGroups = _RegExp.GetGroupNames
	Dim rGroups = rMatch.Groups

	Me.sCallerProcess = rGroups!CallerProcess.Value
	Me.PID = rGroups!PID.Value
	Me.sIndentification = rGroups!Indentification.Value
	Me.EXEFile = rGroups!ExePath.Value
	Me.ActionText = rGroups!Action.Value
	Me.sRuleType = rGroups!RuleType.Value
	Dim sRuleGUID = rGroups!RuleGUID.Value
	Me.RuleGUID = New Guid(sRuleGUID)
	Else
	Throw New NotSupportedException("Не удалось разобрать строку журнала: " & sLogFileRow)
	End If
	End With
*/
#endregion


#region ASYNC_AWAIT SAMPLE
/*

private static async Task<int> Sampe1()
{
	var results = await (new Func<int>(() =>
	{
		Thread.Sleep(1000);
		return 5;
	})).e_RunAsync();
	return results;
}

 Public Sub Main()
 Dim results = Task.WhenAll(SlowCalculation, AnotherSlowCalculation).Result
 For Each result In results
 Console.WriteLine(result)
 Next
 End Sub


private static async Task<int> SlowCalculation()
{
	await Task.Delay(2000); return 40;
}
private static async Task<int> AnotherSlowCalculation()
{
	await Task.Delay(2000); return 20;
}


// TASK(OF T) EXAMPLE
// Async Function TaskOfT_MethodAsync() As Task(Of Integer)

// Task.FromResult Не запускает задачу, а лишь оборачивает резуоттат в обёртку Task(of XX)
// Dim today As String = Await Task.FromResult(Of String)(DateTime.Now.DayOfWeek.ToString())

// The method then can process the result in some way.
// Dim leisureHours As Integer
// If today.First() = "S" Then
// leisureHours = 16
// Else
// leisureHours = 5
// End If

// ' Because the return statement specifies an operand of type Integer, the
// ' method must have a return type of Task(Of Integer).
// Return leisureHours
// End Function



// Public Sub Main()
// Dim tasks = Enumerable.Range(0, 100).Select(AddressOf TurnSlowlyIntegerIntoString)

// Dim resultingStrings = Task.WhenAll(tasks).Result

// For Each value In resultingStrings
// Console.WriteLine(value)
// Next
// End Sub
// Async Function TurnSlowlyIntegerIntoString(input As Integer) As Task(Of String)
// Await Task.Delay(2000)

// Return input.ToString()
// End Function



#region FUNCTION SAMPLE

// Public Class Form1
// Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
// ' ExampleMethodAsync returns a Task.
// Await ExampleMethodAsync()
// TextBox1.Text = vbCrLf & "Control returned to button1_Click."
// End Sub

// Async Function ExampleMethodAsync() As Task
// ' The following line simulates a task-returning asynchronous process.
// Await Task.Delay(1000)
// End Function
// End Class


// Public Class Form1
// Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
// AddHandler Button1.Click,
// Async Sub(sender1, e1)
// ' ExampleMethodAsync returns a Task.
// Await ExampleMethodAsync()
// TextBox1.Text = vbCrLf & "Control returned to Button1_ Click."
// End Sub
// End Sub
// Async Function ExampleMethodAsync() As Task
// ' The following line simulates a task-returning asynchronous process.
// Await Task.Delay(1000)
// End Function
// End Class

// Private Async Sub _Click(sender As Object, e As EventArgs) Handles Button1.Click
// Полный вариант
// 'Dim tsk As New Task(Of Integer)(AddressOf func_for_task)
// 'Call tsk.Start()
// 'Dim val = Await tsk
// 'MsgBox(val)

// Сокращённый вариант
// Dim val = Await Task(Of Integer).Factory.StartNew(AddressOf func_for_task)
// MsgBox(val)
// End Sub

// Private Function func_for_task() As Integer
// Dim val As Integer
// For i = 1 To 999999999
// val += 1
// Next
// Return val
// End Function


// ----------------------------

// Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
// ' Call the method that runs asynchronously.
// Dim result As String = Await WaitAsynchronouslyAsync()

// ' Call the method that runs synchronously.
// 'Dim result As String = Await WaitSynchronously()

// ' Display the result.
// TextBox1.Text &= result
// End Sub

// ' The following method runs asynchronously. The UI thread is not
// ' blocked during the delay. You can move or resize the Form1 window 
// ' while Task.Delay is running.
// Public Async Function WaitAsynchronouslyAsync() As Task(Of String)
// Await Task.Delay(10000)
// Return "Finished"
// End Function

// ' The following method runs synchronously, despite the use of Async.
// ' You cannot move or resize the Form1 window while Thread.Sleep
// ' is running because the UI thread is blocked.
// Public Async Function WaitSynchronously() As Task(Of String)
// ' Import System.Threading for the Sleep method.
// Thread.Sleep(10000)
// Return "Finished"
// End Function



#endregion

#region SUB SAMPLE

// Public Async Function LoadProductsListFromWSUS(rWSUS As IUpdateServer, rWSUSSubscription As ISubscription) As Task
// 'Call DEBUG_SHOW_LINE("Start Loading Products...")

// *** ВАРИАНТ 1
// Using tskAsyncTask As New Task(Sub()
// _Products = WsusProductFamily.BuildProductsTree(rWSUS, rWSUSSubscription)
// 'Global.System.Threading.Thread.Sleep(10000)
// End Sub, TaskCreationOptions.LongRunning)
// Call tskAsyncTask.Start()
// Await tskAsyncTask
// End Using
// 'Call DEBUG_SHOW_LINE("LOADED!")

// *** ВАРИАНТ 2
// 'Call DEBUG_SHOW_LINE("Start Loading Products...")
// Await Task.Run(Sub()
// _Products = WsusProductFamily.BuildProductsTree(rWSUS, rWSUSSubscription)
// 'Global.System.Threading.Thread.Sleep(10000)
// End Sub)
// 'Call DEBUG_SHOW_LINE("LOADED!")
// End Function



// Private Async Sub StartAsyncTask()
// Полный вариант
// Dim tsk As New Task(AddressOf AsyncTaskCallBack)
// tsk.Start()
// MsgBox("Started")
// Await tsk
// MsgBox("Finished")

// Краткий вариант
// MsgBox("Started")
// 'Await Task.Factory.StartNew(AddressOf TTT)
// MsgBox("Finished")

// End Sub

// Private Sub AsyncTaskCallBack()
// Dim val As Integer
// For i = 1 To 999999999
// val += 1
// Next
// End Sub
#endregion


//spinner.IsBusy = true;
//try
//{
//  Task t1 = Task.Run(() => dataX = loadX());
//  Task t2 = Task.Run(() => dataY = loadY());
//  Task t3 = Task.Run(() => dataZ = loadZ());
//  await Task.WhenAll(t1, t2, t3);
//}
//finally
//{
//    spinner.IsBusy = false;
//}


//private SemaphoreSlim _mutex = new SemaphoreSlim(5);
//private HttpClient _client = new HttpClient();
//private async Task<string> DownloadStringAsyncddd(string url)
//{
//    await _mutex.TakeAsync();
//    try
//    {
//        return await _client.GetStringAsync(url);
//    }
//    finally
//    {
//        _mutex.Release();
//    }
//}

//IEnumerable<string> urls = ...;
//var data = await Task.WhenAll(urls.Select(url => DownloadStringAsync(url));





#endregion

#region Use of cancellationToken for async

// Public Async Task StartTimer(CancellationToken cancellationToken)
// {

// await Task.Run(async () =>
// {
// While (True)
// {
// DoSomething();
// await Task.Delay(10000, cancellationToken);
// If (cancellationToken.IsCancellationRequested)
// break;
// }
// });

// }
// When you want to stop the thread just abort the token
// cancellationToken.Cancel();
*/
#endregion






/// <summary>Commnon Tools For Net Multiplatform
/// (C) UOM 2000-2021 </summary>
namespace uom
{

	/// <summary>CodeSamples</summary>
	internal static class CODE_SAMPLES
	{


	}


	/// <summary>Constants</summary>
	internal static partial class constants
	{

		internal const char vbNullChar = '\0';
		internal const char vbCr = '\r';
		internal const char vbLf = '\n';
		internal const char vbTab = '\t';

		internal const string vbCrLf = "\r\n";
		internal const string vbCrLf2 = vbCrLf + vbCrLf;
		internal const string vbCrCrLf = "\r" + vbCrLf;

		internal const char CC_NULL = '\0';
		internal const char CC_EQUAL = '=';
		internal const char CC_SLASH = '\\';
		internal const char CC_SPACE = ' ';

		/// <summary>Символ кавычки "</summary>
		internal const char CC_QUOTE = '"';
		internal static string QUOTE_X(int X) => new(CC_QUOTE, X);

		/// <summary>кавычки ""</summary>
		internal const string CS_QUOTE = "\"";
		/// <summary>Двойные кавычки ""</summary>
		internal static readonly string CS_QUOTE2 = QUOTE_X(2);
		/// <summary>Четверные кавычки """"</summary>
		internal static readonly string CS_QUOTE4 = QUOTE_X(4);



		internal static readonly System.Lazy<Encoding> LEncoding_Windows1251 = new(() =>
		{
#if NETCOREAPP3_1_OR_GREATER //Required In NetCORE
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
			return Encoding.GetEncoding("Windows-1251");
		}
			);

		internal static readonly Lazy<Encoding> LEncoding_cp866 = new(() =>
		{
#if NETCOREAPP3_1_OR_GREATER //Required In NetCORE
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif

			return Encoding.GetEncoding("cp866");
		}
			);


		internal const string CS_SLASH_SLASH = "\\\\";
		internal const string CS_SEPARATOR_10 = "==========";
		internal const string CS_CONSOLE_SEPARATOR = CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10;
		internal const string CS_READY = "Готово.";
		internal const string CS_NONE = "[нет]";
		internal const string CS_ALPHABET_EN = "abcdefghijklmnopqrstuvwxyz";
		internal const string CS_ALPHABET_RU = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
		internal const string CS_ALPHABET_DIGITS = "1234567890";


		/// <summary>Used in any formating functions for format decimal numbers</summary>
		internal const int C_DEFAULT_DECIMAL_DIGITS = 2;

		internal static readonly IntPtr HANDLE_INVALID = new(-1);


		#region Console Const


		#region ASCII Pseudorgaphics
		//https://www.asciitable.com/
		/// <summary>░</summary>
		internal const char CC_ASCII_176 = '░';
		/// <summary>▒</summary>
		internal const char CC_ASCII_177 = '▒';
		/// <summary>▓</summary>
		internal const char CC_ASCII_178 = '▓';
		/// <summary>█</summary>
		internal const char CC_ASCII_219 = '█';
		#endregion


		internal const char CC_PROGRESSBAR_EMPTY = '_';
		internal const char CC_PROGRESSBAR_FULL = '#';

		/// <summary>░</summary>
		internal const char CC_ASCII_PROGRESSBAR_EMPTY = CC_ASCII_176;
		/// <summary>▓</summary>
		internal const char CC_ASCII_PROGRESSBAR_FULL = CC_ASCII_178;

		internal const string CS_ENTER_PWD_EN = "Enter password: ";
		internal const int C_DEFAULT_CONSOLE_WIDTH = 80;
		internal const int C_DEFAULT_CONSOLE_WIDTH_1 = C_DEFAULT_CONSOLE_WIDTH - 1;

		#endregion


		/// <summary>Получаем системный символ-разделитель байтов в HEX-строке</summary>
		private static char? _cSystemDefaultHexByteSeparator = null;
		private static readonly EventArgs _SystemDefaultHexByteSeparatorLock = new();
		/// <summary>Standart system byte seperator char</summary>
		internal static char SystemDefaultHexByteSeparator
		{
			get
			{
				lock (_SystemDefaultHexByteSeparatorLock)
				{
					var sSample = BitConverter.ToString(new Byte[] { 1, 2 });
					_cSystemDefaultHexByteSeparator ??= Convert.ToChar(BitConverter.ToString(new Byte[] { 1, 2 }).Substring(2, 1));
					return (char)_cSystemDefaultHexByteSeparator!;
				}
			}
		}



		// Поиск в многострочном тексте
		internal const System.Text.RegularExpressions.RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline = System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant;

		// Поиск в многострочном тексте, в каждой строке по-отдельности
		internal const System.Text.RegularExpressions.RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline = System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.CultureInvariant;

		// Поиск в многострочном тексте, и в каждой строке по-отдельности
		internal const System.Text.RegularExpressions.RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline_Singleline = System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant;


		//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute ExtGlb_GetCountryPhoneFormat(this UOM.Globalization_.mGlobalization.ECounties CNT)
		//{
		//    return UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute.GetCountryPhoneFormat(CNT);
		//}

		///// <summary>Выделяет из строки номер телефона</summary>
		//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static string ExtGlb_PhoneParse(this string sPhone, UOM.Globalization_.mGlobalization.ECounties CNT = UOM.Globalization_.mGlobalization.ECounties._UNKNOWN)
		//{
		//    return UOM.Globalization_.mGlobalization.PhoneParse(sPhone, CNT);
		//}

		public enum E_STRING_TYPES : int
		{
			Auto = 0,
			Uni,
			Ansi
		}


		/// <summary>Миллиметров в дюйме</summary>
		internal const float C_MM_IN_INCH = 25.4f;

		/// <summary>Сантиметров в дюйме</summary>
		internal const float C_CM_IN_INCH = C_MM_IN_INCH / 10f;
		internal const string C_FMT_LONGNUMBER = "### ### ### ### ### ### ### ### ### ### ### ### ### ### ##0";
		internal const string C_YES_ENG = "Yes";
		internal const string C_NO_ENG = "No";
		internal const string C_YES_RUS = "Да";
		internal const string C_NO_RUS = "Нет";





	}


	#region Structures _Int16/32/64

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int16
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public short Word = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public ushort UWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte LoByte = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte HiByte = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;

		#region CORE

		public _Int16(short V) => Word = V;
		public _Int16(ushort V) => UWord = V;

		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1 };
			set
			{
				if (value.Length != 2) throw new ArgumentException($"{nameof(_Int16)} Constructor failed! byteArray lenght is wrong!");
				Byte_0 = value[0];
				Byte_1 = value[1];
			}
		}

		#endregion

		public override string ToString() => (UWord.ToString() + " / " + Bytes.e_ToStringHex(true));

		#region Conversions

		public static implicit operator ushort(_Int16 L) => L.UWord;
		public static implicit operator short(_Int16 L) => L.Word;
		public static explicit operator _Int16(ushort L) => new(L);
		public static explicit operator _Int16(short L) => new(L);
		public static explicit operator _Int16(byte I) => new(0) { LoByte = I };

		#endregion

		#region Operators
		public static bool operator <(_Int16 I1, _Int16 I2) => I1.UWord < I2.UWord;
		public static bool operator >(_Int16 I1, _Int16 I2) => I1.UWord > I2.UWord;
		public static bool operator ==(_Int16 I1, _Int16 I2) => I1.UWord == I2.UWord;
		public static bool operator !=(_Int16 I1, _Int16 I2) => I1.UWord != I2.UWord;
		public static bool operator <(_Int16 I1, ushort I2) => I1.UWord < I2;
		public static bool operator >(_Int16 I1, ushort I2) => I1.UWord > I2;
		public static bool operator ==(_Int16 I1, ushort I2) => I1.UWord == I2;
		public static bool operator !=(_Int16 I1, ushort I2) => I1.UWord != I2;
		public static _Int16 operator -(_Int16 I1, _Int16 I2) => new((ushort)(I1.UWord - I2.UWord));
		public static _Int16 operator -(_Int16 I1, ushort I2) => new((ushort)(I1.UWord - I2));
		public static _Int16 operator +(_Int16 I1, _Int16 I2) => new((ushort)(I1.UWord + I2.UWord));
		public static _Int16 operator +(_Int16 I1, ushort I2) => new((ushort)(I1.UWord + I2));
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals(object obj) => (this == (_Int16)obj);
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode() => UWord.GetHashCode();
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int32
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public int DWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public uint UDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.I2)] public short LoWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U2)] public ushort ULoWord = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.I2)] public short HiWord = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U2)] public ushort UHiWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.LPStruct)] public _Int16 LoWord16 = new(0);
		[FieldOffset(2)][MarshalAs(UnmanagedType.LPStruct)] public _Int16 HiWord16 = new(0);
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset(3)][MarshalAs(UnmanagedType.U1)] public byte Byte_3 = 0;

		#region CORE

		public _Int32(int V) { DWord = V; }
		public _Int32(uint V) { UDWord = V; }

		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1, Byte_2, Byte_3 };
			set
			{
				if (value.Length != 4) throw new ArgumentException($"{nameof(_Int32)} Constructor failed! byteArray lenght is wrong!");
				Byte_0 = value[0];
				Byte_1 = value[1];
				Byte_2 = value[2];
				Byte_3 = value[3];
			}
		}

		#endregion
		public override string ToString() => (UDWord.ToString() + " / " + Bytes.e_ToStringHex(true));

		#region Conversions
		public static implicit operator uint(_Int32 L) => L.UDWord;
		public static implicit operator int(_Int32 L) => L.DWord;
		public static explicit operator _Int32(uint L) => new(L);
		public static explicit operator _Int32(int L) => new(L);
		public static explicit operator _Int32(short I) => new(0) { LoWord = I };
		public static explicit operator _Int32(ushort I) => new(0) { ULoWord = I };
		#endregion

		#region Operators
		public static bool operator <(_Int32 I1, _Int32 I2) => I1.UDWord < I2.UDWord;
		public static bool operator >(_Int32 I1, _Int32 I2) => I1.UDWord > I2.UDWord;
		public static bool operator ==(_Int32 I1, _Int32 I2) => I1.UDWord == I2.UDWord;
		public static bool operator !=(_Int32 I1, _Int32 I2) => I1.UDWord != I2.UDWord;
		public static bool operator <(_Int32 I1, uint I2) => I1.UDWord < I2;
		public static bool operator >(_Int32 I1, uint I2) => I1.UDWord > I2;
		public static bool operator ==(_Int32 I1, uint I2) => I1.UDWord == I2;
		public static bool operator !=(_Int32 I1, uint I2) => I1.UDWord != I2;
		public static _Int32 operator -(_Int32 I1, _Int32 I2) => new(I1.UDWord - I2.UDWord);
		public static _Int32 operator -(_Int32 I1, uint I2) => new(I1.UDWord - I2);
		public static _Int32 operator +(_Int32 I1, _Int32 I2) => new(I1.UDWord + I2.UDWord);
		public static _Int32 operator +(_Int32 I1, uint I2) => new(I1.UDWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals(object obj) => (this == (_Int32)obj);
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode() => UDWord.GetHashCode();
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int64
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I8)] public long QWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U8)] public ulong UQWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public int LoDWord = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.I4)] public int HiDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public uint ULoDWord = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.U4)] public uint UHiDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.LPStruct)] public _Int32 LoDWord32 = new(0);
		[FieldOffset(4)][MarshalAs(UnmanagedType.LPStruct)] public _Int32 HiDWord32 = new(0);
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset(3)][MarshalAs(UnmanagedType.U1)] public byte Byte_3 = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.U1)] public byte Byte_4 = 0;
		[FieldOffset(5)][MarshalAs(UnmanagedType.U1)] public byte Byte_5 = 0;
		[FieldOffset(6)][MarshalAs(UnmanagedType.U1)] public byte Byte_6 = 0;
		[FieldOffset(7)][MarshalAs(UnmanagedType.U1)] public byte Byte_7 = 0;

		#region CORE

		public _Int64(long V) => QWord = V;
		public _Int64(ulong V) => UQWord = V;

		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1, Byte_2, Byte_3, Byte_4, Byte_5, Byte_6, Byte_7 };
			set
			{
				if (value.Length != 8) throw new ArgumentException($"{nameof(_Int64)} Constructor failed! byteArray lenght is wrong!");
				Byte_0 = value[0];
				Byte_1 = value[1];
				Byte_2 = value[2];
				Byte_3 = value[3];
				Byte_4 = value[4];
				Byte_5 = value[5];
				Byte_6 = value[6];
				Byte_7 = value[7];
			}
		}
		#endregion

		public override string ToString() => UQWord.ToString() + " / " + Bytes.e_ToStringHex(true);

		#region Conversions
		public static implicit operator ulong(_Int64 L) => L.UQWord;
		public static implicit operator long(_Int64 L) => L.QWord;
		public static explicit operator _Int64(ulong L) => new(L);
		public static explicit operator _Int64(long L) => new(L);
		public static explicit operator _Int64(int I) => new(0L) { LoDWord = I };
		public static explicit operator _Int64(uint I) => new(0L) { ULoDWord = I };
		#endregion

		#region Operators

		public static bool operator <(_Int64 I1, _Int64 I2) => I1.UQWord < I2.UQWord;
		public static bool operator >(_Int64 I1, _Int64 I2) => I1.UQWord > I2.UQWord;
		public static bool operator ==(_Int64 I1, _Int64 I2) => I1.UQWord == I2.UQWord;
		public static bool operator !=(_Int64 I1, _Int64 I2) => I1.UQWord != I2.UQWord;
		public static bool operator <(_Int64 I1, ulong I2) => I1.UQWord < I2;
		public static bool operator >(_Int64 I1, ulong I2) => I1.UQWord > I2;
		public static bool operator ==(_Int64 I1, ulong I2) => I1.UQWord == I2;
		public static bool operator !=(_Int64 I1, ulong I2) => I1.UQWord != I2;
		public static _Int64 operator -(_Int64 I1, _Int64 I2) => new(I1.UQWord - I2.UQWord);
		public static _Int64 operator -(_Int64 I1, ulong I2) => new(I1.UQWord - I2);
		public static _Int64 operator +(_Int64 I1, _Int64 I2) => new(I1.UQWord + I2.UQWord);
		public static _Int64 operator +(_Int64 I1, ulong I2) => new(I1.UQWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals(object obj) => (this == (_Int64)obj);
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode() => UQWord.GetHashCode();

	}


	#endregion


	/// <summary>Application Tools</summary>
	internal static class OSInfo
	{
		public static bool IsOSPlatform_Windows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		public static bool IsOSPlatform_OSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	}



	/// <summary>Info about application assembly</summary>
	internal static partial class AppInfo
	{
		public static Assembly Assembly => Assembly.GetExecutingAssembly();

		public static string? Title => Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
		public static string? Company => Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
		public static string? Description => Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
		public static string? Copyright => Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
		public static string? Product => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>()?.Product;
		public static string? ProductVersion => AssemblyFileVersionInfo?.ProductVersion;
		public static string? Comments => AssemblyFileVersionInfo?.Comments;
		public static string? Trademark => Assembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark;
		public static string? Version => Assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;


		public static FileInfo File => new(Assembly.Location);
		public static string? FileVersion => Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
		public static FileVersionInfo AssemblyFileVersionInfo => FileVersionInfo.GetVersionInfo(Assembly.Location);


		////public static string? FileVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		//public static string? FileVersion()
		//{
		//    var FVI = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
		//    return FVI.FileVersion;
		//}
		public static string? Configuration => Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
		public static uint? AlgorithmId => Assembly.GetCustomAttribute<AssemblyAlgorithmIdAttribute>()?.AlgorithmId;
		public static string? Culture => Assembly.GetCustomAttribute<AssemblyCultureAttribute>()?.Culture;
		public static AssemblyNameFlags? Flags => (AssemblyNameFlags?)(Assembly.GetCustomAttribute<AssemblyFlagsAttribute>()?.AssemblyFlags);



		public static T? GetAssemblyAttribute<T>() where T : Attribute => GetAssemblyAttribute<T>(Assembly);

		public static T? GetAssemblyAttribute<T>(Assembly asmbl) where T : Attribute
		{
			var attributes = asmbl.GetCustomAttributes(typeof(T), true);
			return (null == attributes || !attributes!.Any()) ? null : (T)attributes.First();
		}





		/// <summary>в режиме дизайнера WPF?</summary>
		private static bool IsInDesignerMode_WPF => (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

		/// <summary>в режиме дизайнера WinForms?</summary>
		private static bool IsInDesignerMode_WinForms => (Process.GetCurrentProcess().ProcessName.ToLower() ?? "") == ("devenv".ToLower());


		public static string? TitleHeader => Title + ((FileVersion == null) ? "" : " v" + FileVersion);

		public static bool CurrentUICultureIsRuTree => CultureInfo.CurrentUICulture.e_IsRussianTree();




	}



	internal class DateTimeInterval : Stopwatch
	{

		[Flags]
		private enum TimeParts : int
		{
			Start = 1,
			Stop = 2
		}
		public DateTime StartTime { get; set; } = DateTime.Now;
		public DateTime StopTime { get; set; } = DateTime.Now;

		DateTimeInterval() : base() { UpdateFromTime(); }

		public override string ToString() => $"с {StartTime} по {StopTime}";

		private void UpdateFromTime(TimeParts flg = TimeParts.Start | TimeParts.Stop)
		{
			var dtNow = DateTime.Now;
			if (flg.HasFlag(TimeParts.Start)) StartTime = dtNow;
			if (flg.HasFlag(TimeParts.Stop)) StopTime = dtNow;
		}

		/// <summary>Starts, or resumes, measuring elapsed time for an interval.</summary>
		public new void Start()
		{
			base.Start();
			UpdateFromTime(TimeParts.Start);
		}

		/// <summary>Initializes a new System.Diagnostics.Stopwatch instance, sets the elapsed time property to zero, and starts measuring elapsed time.</summary>
		public static new DateTimeInterval StartNew()
		{
			var DTI = new DateTimeInterval();
			DTI.Start();
			return DTI;
		}

		/// <summary>Stops measuring elapsed time for an interval.</summary>
		public new void Stop()
		{
			base.Stop();
			UpdateFromTime(TimeParts.Stop);
		}


		/// <summary>Stops time interval measurement and resets the elapsed time to zero.</summary>
		public new void Reset()
		{
			base.Reset();
			UpdateFromTime();
		}

		/// <summary>Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.</summary>
		public new void Restart()
		{
			Stop();
			Start();
		}
	}

	/// <summary> Base Class than automaticaly disposes its any attached values </summary>
	internal abstract partial class AutoDisposableUniversal : IDisposable
	{

		#region IDisposable Support
		private bool disposedValue;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					try { FreeManagedObjects(); } catch { }// Ignore Any Errors
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer                
				try { FreeUnmanagedObjects(); } catch { }// Ignore Any Errors    

				disposedValue = true;
			}
		}

		//// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~AutoDisposableUniversal()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion


		protected Stack<IDisposable> ManagedObjectsToDispose { get; private set; } = new Stack<IDisposable>();
		protected Stack<IDisposable> UnManagedObjectsToDispose { get; private set; } = new Stack<IDisposable>();
		protected Stack<Action> ManagedDisposeCallBacks { get; private set; } = new Stack<Action>();
		protected Stack<Action> UnManagedDisposeCallBacks { get; private set; } = new Stack<Action>();

		public AutoDisposableUniversal(Action? rManagedDisposeCallBack = null, Action? rUnManagedDisposeCallBack = null) : base()
		{
			if (null != rManagedDisposeCallBack) RegisterDisposeCallback(rManagedDisposeCallBack);
			if (null != rUnManagedDisposeCallBack) RegisterDisposeCallback(rUnManagedDisposeCallBack);
		}

		/// <summary>Регистрируем объекты, которые надо будет уничтожить, при уничтожении родительского объекта</summary>
		protected internal void RegisterDisposableObject(IDisposable MDO, bool IsManaged = true)
		{
			_ = MDO ?? throw new ArgumentNullException(nameof(MDO));
			if (ManagedObjectsToDispose.Contains(MDO) || UnManagedObjectsToDispose.Contains(MDO)) throw new ArgumentException("Object already added to dispose list!", nameof(MDO));

			var rListToAdd = UnManagedObjectsToDispose;
			if (IsManaged) rListToAdd = ManagedObjectsToDispose;
			rListToAdd.Push(MDO);
		}


		/// <summary>Регистрируем действия, которые надо будет выполнить при уничтожении родительского объекта</summary>
		protected internal void RegisterDisposeCallback(Action onDispose, bool IsManaged = true)
		{
			_ = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
			if (ManagedDisposeCallBacks.Contains(onDispose) || UnManagedDisposeCallBacks.Contains(onDispose))
				throw new ArgumentException($"'{nameof(onDispose)}' Already regidtered!", nameof(onDispose));

			if (IsManaged)
				ManagedDisposeCallBacks.Push(onDispose);
			else
				UnManagedDisposeCallBacks.Push(onDispose);
		}

		protected virtual void FreeManagedObjects()
		{
			OnBeforeDispose(true);
			DisposeList(ManagedObjectsToDispose);
			RunDisposeActions(ManagedDisposeCallBacks);
		}

		protected virtual void FreeUnmanagedObjects()
		{
			OnBeforeDispose(false);
			DisposeList(UnManagedObjectsToDispose);
			RunDisposeActions(UnManagedDisposeCallBacks);
		}

		/// <summary>Just template, override if need</summary>            
		protected virtual void OnBeforeDispose(bool bManages) { }

		private static void DisposeList(Stack<IDisposable> rList)
		{
			while (rList.Any())
			{
				var rObjectToKill = rList.Pop();
				rObjectToKill.e_DisposeAndSetNothing();
			}
		}

		private static void RunDisposeActions(Stack<Action> rList)
		{
			while (rList.Any())
			{
				var A = rList.Pop();
				A.Invoke();
			}
		}


	}

	/// <summary> Class than automaticaly disposes 1 attached value </summary>
	internal abstract partial class AutoDisposable1 : AutoDisposableUniversal
	{
		public AutoDisposable1() : base() { }

		protected IDisposable? _Value = null;
		public IDisposable? Value
		{
			get => _Value;
			set { _Value = value; RegisterDisposableObject(_Value!, true); }
		}
	}

	/// <summary> Class than automaticaly disposes 1 attached value </summary>
	internal partial class AutoDisposable1T<T> : AutoDisposableUniversal where T : IDisposable
	{
		public AutoDisposable1T() : base() { }

		protected T? _Value = default;
		public T? Value
		{
			get => _Value;
			set { _Value = value; RegisterDisposableObject(_Value!, true); }
		}
	}




	/// <summary>Container for multithread safe get / set property value
	/// Supports notification on property changes (doe not check real changing of value, just notify about SET calls)</summary>
	/// <remarks>Valid for simple types (int, string, boolean), class and struts will not be MT safe, bc their child props has no MTsafe protect</remarks>
	[DefaultProperty("Value")]
	internal abstract partial class MTSafeContainerBase<T> : AutoDisposableUniversal
	{

		#region ValueChangedEventArgs
		public partial class ValueChangedEventArgs : EventArgs
		{
			public readonly T? OldValue = default;
			public readonly T? NewValue = default;
			public bool Cancel = false;

			public ValueChangedEventArgs(T? rOldValue, T? rNewValue) : base()
			{
				OldValue = rOldValue;
				NewValue = rNewValue;
				Cancel = false;
			}
		}
		#endregion

		public event EventHandler<ValueChangedEventArgs>? BeforeValueChanged;
		public event EventHandler<ValueChangedEventArgs>? AfterValueChanged;
		public event EventHandler<ValueChangedEventArgs>? ValueChangeCanceled;

		/// <summary>MT-UNsafe</summary>
		private T? _UnsafeValue = default;

		/// <summary>MT-UNsafe</summary>
		protected T? UnsafeValue
		{
			get => _UnsafeValue;
			set
			{
				var EA = new ValueChangedEventArgs(_UnsafeValue, value);
				OnBeforeValueChanged(EA);
				if (EA.Cancel) OnValueChangeCanceled(EA);
				else
				{
					_UnsafeValue = value;
					OnAfterValueChanged(EA);
				}
			}
		}

		public abstract T? Value { get; set; }
		public void SetValue(T NewValue) => Value = NewValue;

		public override string ToString() => $"{typeof(T)} = {Value}";

		protected void OnBeforeValueChanged(ValueChangedEventArgs EA) => BeforeValueChanged?.Invoke(this, EA);
		protected void OnValueChangeCanceled(ValueChangedEventArgs EA) => ValueChangeCanceled?.Invoke(this, EA);
		protected void OnAfterValueChanged(ValueChangedEventArgs EA) => AfterValueChanged?.Invoke(this, EA);




	}

	internal partial class MTSafeContainerSyncLock<T> : MTSafeContainerBase<T>
	{
		protected EventArgs _MTSyncObject = new();

		public MTSafeContainerSyncLock(T InitialValue) : base() => Value = InitialValue;

		public override T? Value
		{
			get { lock (_MTSyncObject) return UnsafeValue; }
			set { lock (_MTSyncObject) UnsafeValue = value; }
		}
	}

	/// <summary>'Threading.ReaderWriterLockSlim' Контейнер для многопоточно-безопасного получения и установки какого либо значения
	/// Позволяет множественные чтения, но эксклюзивную запись
	/// Поддерживает уведомления об изменении свойств (не проверяет реально ли значение изменилось, просто срабатывает после каждой установки значения)</summary>
	/// <typeparam name="T">Тип значения в контейнере</typeparam>
	/// <remarks>Подходит для простых типов (int, string, boolean), составные типы или классы не будут безопасными, т.к. их дочерние свойства и поля не защищаются данным механизмом</remarks>
	internal partial class MTSafeContainer<T> : MTSafeContainerBase<T>
	{
		public ReaderWriterLockSlim? MTSyncObject { get; private set; } = null;

		public MTSafeContainer(T InitialValue, LockRecursionPolicy LRP = LockRecursionPolicy.NoRecursion) : base()
		{
			MTSyncObject = new ReaderWriterLockSlim(LRP);
			RegisterDisposeCallback(Destroy);
			Value = InitialValue;
		}

		protected void RunInSafeLock(Action a, bool write)
		{
			if (write)
				MTSyncObject?.EnterWriteLock();
			else
				MTSyncObject?.EnterReadLock();

			try
			{
				a.Invoke();
			}
			finally
			{
				if (write)
					MTSyncObject?.ExitWriteLock();
				else
					MTSyncObject?.ExitReadLock();
			}
		}

		/// <summary>Многопоточнобезопасное получение и установка значения</summary>
		public override T? Value
		{
			get
			{
				T? v = default(T?);
				RunInSafeLock(() => { v = UnsafeValue; }, false);
				return v;
			}
			set
			{
				RunInSafeLock(() => { UnsafeValue = value; }, true);
			}
		}

		/// <summary> IDisposable</summary>
		private void Destroy() => MTSyncObject?.e_DisposeAndSetNothing();
	}

	//[DefaultProperty("IsSet")]
	internal partial class MTSafeBooleanFlag : MTSafeContainer<bool>
	{
		public MTSafeBooleanFlag(bool bDefaultValue = false) : base(bDefaultValue) { }

		public void SetlFlag(bool bFlagValue = true) => Value = bFlagValue;
		public void ClearFlag() => Value = false;
		/// <summary>Инвертирует текущее состояние. Возвращает новое, установленное состояние</summary>
		public bool Invert() { bool bInverted = !Value; Value = bInverted; return bInverted; }
		public bool IsSet { get => Value; }

		public static bool operator true(MTSafeBooleanFlag R) => R.Value;
		public static bool operator false(MTSafeBooleanFlag R) => !R.Value;

		public static implicit operator bool(MTSafeBooleanFlag d) => d.IsSet;

		public static implicit operator MTSafeBooleanFlag(bool bFlag) => new(bFlag);
	}

	internal partial class MTSafeCounterInt32 : MTSafeContainer<int>
	{
		public MTSafeCounterInt32(int iDefaultValue = 0) : base(iDefaultValue) { }

		public void Increment(int iStep = 1)
			=> RunInSafeLock(() => { UnsafeValue += iStep; }, true);

		public void Decrement(int iStep = 1)
			=> RunInSafeLock(() => { UnsafeValue -= iStep; }, true);

		public void Reset() => Value = 0;

		public static implicit operator int(MTSafeCounterInt32 I) => I.Value;
		public static implicit operator MTSafeCounterInt32(int I) => new(I);
		public static bool operator <(MTSafeCounterInt32 I1, int I2) => (I1 < I2);
		public static bool operator <=(MTSafeCounterInt32 I1, int I2) => (I1 <= I2);
		public static bool operator >(MTSafeCounterInt32 I1, int I2) => (I1 > I2);
		public static bool operator >=(MTSafeCounterInt32 I1, int I2) => (I1 >= I2);
		public static bool operator ==(MTSafeCounterInt32 I1, int I2) => (I1 == I2);
		public static bool operator !=(MTSafeCounterInt32 I1, int I2) => (I1 != I2);

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals(object obj) => (this == (MTSafeCounterInt32)obj);
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode() => Value.GetHashCode();
	}


	/// <summary>Очередь для уничтожаемых одбектов, которые надо не забыть уничтожить, но прямо сейчас их уничтожать нельзя.
	/// Например для текущего захваченного с камеры кадра, который в данный момент отображается на экране - его нельзя сейчас уничтожать,
	/// но при добовлении следующего кадру, этот старый уже можно уничтожать, и его надо где-то хранить - для этого эта помойка.</summary>
	internal class DisposableTrashBin : AutoDisposableUniversal
	{

		private readonly EventArgs _lock = new();
		private readonly Queue<IDisposable> _Q = new();
		private readonly int _iQueueLength = 10;

		public DisposableTrashBin(int QueueLength) : base()
		{
			_iQueueLength = QueueLength;

			RegisterDisposeCallback(() => ClearAll());
		}

		public void Put(IDisposable ObjectToDispose)
		{
			if (ObjectToDispose == null) return;

			lock (_lock)
			{
				if (_Q.Contains(ObjectToDispose)) return;

				_Q.Enqueue(ObjectToDispose);

				while (_Q.Count > _iQueueLength)
				{
					ObjectToDispose = _Q.Dequeue();
					ObjectToDispose.Dispose();

					//Debug.WriteLine($"Trash disposed old object, queue = {_Q.Count}");
				}
			}
		}

		public void ClearAll()
		{
			lock (_lock)
			{
				while (_Q.Count > 0)
				{
					var ObjectToDispose = _Q.Dequeue();
					try { ObjectToDispose.Dispose(); }
					catch { }//Ignore any errors
				}
			}
			//Debug.WriteLine($"Trash bin cleared.");
		}
	}


	internal static class Asyncs
	{

		// Provides a task scheduler that ensures a maximum concurrency level while running on top of the thread pool.
		public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
		{
			/// <summary>Indicates whether the current thread is processing work items.</summary>
			[ThreadStatic]
			private static bool _currentThreadIsProcessingItems;

			/// <summary>The list of tasks to be e_runuted</summary>
			private readonly LinkedList<Task> _tasks = new(); // protected by lock(_tasks)

			/// <summary>The maximum concurrency level allowed by this scheduler.</summary>
			private readonly int _maxDegreeOfParallelism;

			/// <summary>Indicates whether the scheduler is currently processing work items.</summary>
			private int _delegatesQueuedOrRunning = 0;

			/// <summary>Creates a new instance with the specified degree of parallelism.</summary>
			public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
			{
				if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
				_maxDegreeOfParallelism = maxDegreeOfParallelism;
			}

			/// <summary>Queues a task to the scheduler.</summary>
			protected sealed override void QueueTask(Task task)
			{
				// Add the task to the list of tasks to be processed.  If there aren't enough
				// delegates currently queued or running to process tasks, schedule another.
				lock (_tasks)
				{
					_tasks.AddLast(task);
					if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
					{
						++_delegatesQueuedOrRunning;
						NotifyThreadPoolOfPendingWork();
					}
				}
			}

			// Inform the ThreadPool that there's work to be e_runuted for this scheduler.
			private void NotifyThreadPoolOfPendingWork()
			{
				ThreadPool.UnsafeQueueUserWorkItem(_ =>
				{
					// Note that the current thread is now processing work items.
					// This is necessary to enable inlining of tasks into this thread.
					_currentThreadIsProcessingItems = true;
					try
					{
						// Process all available items in the queue.
						while (true)
						{
							Task item;
							lock (_tasks)
							{
								// When there are no more items to be processed, note that we're done processing, and get out.
								if (_tasks.Count == 0)
								{
									--_delegatesQueuedOrRunning;
									break;
								}

								// Get the next item from the queue
								item = _tasks.First!.Value;
								_tasks.RemoveFirst();
							}

							// e_runute the task we pulled out of the queue
							base.TryExecuteTask(item);
						}
					}
					// We're done processing items on the current thread
					finally { _currentThreadIsProcessingItems = false; }
				}, null);
			}

			/// <summary>Attempts to e_runute the specified task on the current thread.</summary>
			protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
			{
				// If this thread isn't already processing a task, we don't support inlining
				if (!_currentThreadIsProcessingItems) return false;

				// If the task was previously queued, remove it from the queue
				if (taskWasPreviouslyQueued)

					// Try to run the task.            
					if (TryDequeue(task)) return base.TryExecuteTask(task);
					else return false;

				else
					return base.TryExecuteTask(task);
			}

			/// <summary>Attempt to remove a previously scheduled task from the scheduler.</summary>
			protected sealed override bool TryDequeue(Task task) { lock (_tasks) return _tasks.Remove(task); }

			/// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
			public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

			/// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary>
			protected sealed override IEnumerable<Task> GetScheduledTasks()
			{
				bool lockTaken = false;
				try
				{
					Monitor.TryEnter(_tasks, ref lockTaken);
					if (lockTaken) return _tasks;
					else throw new NotSupportedException();
				}
				finally
				{
					if (lockTaken) Monitor.Exit(_tasks);
				}
			}

			public TaskFactory CreateTaskFactory() => new(this);
		}




	}


	internal static class Global
	{
		internal struct CURRENCY
		{
			private RegionInfo _ri;
			public CURRENCY(RegionInfo ri) { _ri = ri; }

			public string ISOCurrencySymbol => _ri.ISOCurrencySymbol;
			public string CurrencySymbol => _ri.CurrencySymbol;
			public string CurrencyEnglishName => _ri.CurrencyEnglishName;
		}

		/// <summary>
		/// AUD, $, Australian Dollar
		/// CAD, $, Canadian Dollar
		/// EUR, ?, Euro
		/// GBP, £, British Pound
		/// JPY, ¥, Japanese Yen
		/// USD, $, US Dollar
		/// </summary>
		internal static CURRENCY[] GetCurrencies()
		{
			return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				.Select(ci => ci.LCID).Distinct()
				.Select(id => new RegionInfo(id))
				.GroupBy(r => r.ISOCurrencySymbol)
				.Select(g => g.First())
				.Select(r => new CURRENCY(r)
				{
				}).ToArray();
		}

		/* Список валют и культур 1 уровня для этой валюты
        var culturesByCurrency = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Where(ci => ci.Parent == CultureInfo.InvariantCulture)
            .Select(ci => new { ci, ci.NumberFormat.CurrencySymbol })
            .Where(x => !string.IsNullOrWhiteSpace(x.CurrencySymbol))
            .GroupBy(x => x.CurrencySymbol)
            .Select(x => new { Symbol = x.First().CurrencySymbol, Cultures = x.Select(z => z.ci).ToArray() })
            .ToDictionary(x => x.Symbol, x => x.Cultures);
        */




	}


	internal static class I_O
	{
		/// <summary>Для файлового ввода-вывода префикс «\\?\» указывает API-интерфейсам Windows отключить весь синтаксический анализ строки и отправить строку, 
		/// которая следует за ней, прямо в файловую систему.
		/// Это позволяет использовать . или .. в именах путей, а также ослабить ограничение в 
		/// 260 символов для имени пути, если базовая файловая система поддерживает длинные пути и имена файлов.
		/// 
		/// https://docs.microsoft.com/ru-ru/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN
		/// 
		/// Так как он отключает автоматическое расширение строки пути, \\префикс "?\" также позволяет использовать ".." и "." в именах путей, 
		/// которые могут быть полезны при попытке выполнить операции с файлом, в противном случае зарезервированные описатели относительных путей в составе полного пути.
		/// 
		/// Многие, но не все API-интерфейсы файлового ввода/вывода поддерживают "\\?\"; для проверки каждого API следует обратиться к справочному разделу.
		/// </summary>
		internal const string CS_PATH_PREFIX_WIN_LONG_PATH = @"\\?\";

		/// <summary> при работе с функциями API Windows следует использовать \\.\ для доступа только к устройствам, а не файлам!.
		/// Большинство интерфейсов API не поддерживают "\\.\";
		/// только те, которые предназначены для работы с пространством имен устройства, распознают его.
		/// Всегда проверяйте справочный раздел для каждого API, чтобы быть уверенным в этом.
		/// </summary>
		internal const string CS_PATH_PREFIX_WIN_DEVICE = @"\\.\";//" \\.\COM56 etc



		/// <summary>Multithread File system scanner</summary>
		internal abstract class FileSystemScannerBase
		{
			public enum ERROR_SOURCES
			{
				Unknown,
				FAILED_GET_FOLDER_CONTENT,
				FAILED_READ_FILE
				//FAILED_GET_CHILD_DIRECTORIES,
			}
			public struct SCAN_ERROR
			{
				public readonly FileSystemInfo fsi;
				public readonly Exception ex;
				public readonly ERROR_SOURCES Source;

				public SCAN_ERROR(FileSystemInfo fso, Exception ex, ERROR_SOURCES src)
				{
					fsi = fso;
					this.ex = ex;
					Source = src;
				}
				public override string ToString() => $"{fsi.e_FullName_RemoveLongPathPrefix()} {Source} {ex.Message.Trim()}";
			}

			public struct SCAN_RUNNING_INFO
			{
				public readonly int TasksToWait;
				public readonly int TasksFinished;

				public SCAN_RUNNING_INFO(int iTasksToWait, int iTasksFinished)
				{
					TasksToWait = iTasksToWait;
					TasksFinished = iTasksFinished;
				}
			}


			private Asyncs.LimitedConcurrencyLevelTaskScheduler? _lcts = null;
			private TaskFactory? _TF = null;

			protected List<Task> _lTasks = new();
			protected CancellationToken _cts = new();

			public CancellationToken StopFlag { get => _cts; }

			protected FileSystemScannerBase() { }


			protected readonly uom.MTSafeCounterInt32 ThreadsCounter = new();

			protected void Scan(
				DirectoryInfo[]? aRootDirsToScan = null,
				int? maxDegreeOfParallelism = null
				)
			{





				if (null == aRootDirsToScan || !aRootDirsToScan.Any())
				{
					//Not specidef Root Dirs for scan
					var aDisks = Environment.GetLogicalDrives();
					if (!aDisks.Any()) throw new Exception("Failed to get logical drives!");
					aRootDirsToScan = (from sDir in aDisks
									   orderby sDir
									   let fiDir = sDir.e_ToDirectoryInfo(true)
									   select fiDir).ToArray();
				}



				maxDegreeOfParallelism ??= Environment.ProcessorCount;

				_cts = new CancellationToken();
				_lcts = new Asyncs.LimitedConcurrencyLevelTaskScheduler((int)maxDegreeOfParallelism);
				_TF = _lcts.CreateTaskFactory();
				_lTasks = new();

				//Start Main Scan Core
				ScanDirs(aRootDirsToScan);

				//sleep some time for backgroubd jobs starts...
				Thread.Sleep(500);

				//Wait all tasks finished...
				var iTasksToWait = 1;
				while (iTasksToWait > 0)
				{
					var iTaskTotal = 0;
					var iTasksFinished = 0;
					lock (_lTasks)
					{
						iTaskTotal = _lTasks.Count;
						//Remove all finished tasks
						_lTasks.RemoveAll((T) => T.Status == TaskStatus.RanToCompletion);
						iTasksToWait = _lTasks.Count;
						iTasksFinished = (iTaskTotal - iTasksToWait);
					}
					OnWaitTasks(new SCAN_RUNNING_INFO(iTasksToWait, iTasksFinished));
				}

			}

			private void ScanDirs(DirectoryInfo[] aDirs)
			{
				if (_cts.IsCancellationRequested) return;

				aDirs.e_ForEach((d) =>
				{
					if (_cts.IsCancellationRequested) return;

					lock (_lTasks)
					{
						var tskNew = _TF!.StartNew(
							() => CheckDir(d),
							_cts,
							TaskCreationOptions.LongRunning, _lcts!);

						_lTasks.Add(tskNew);
					}
				});
			}

			private void CheckDir(DirectoryInfo fiDir)
			{
				ThreadsCounter.Increment();
				try
				{


					//lock (_ConsoleLock) Console.WriteLine($"Start scan dir '{fiDir.FullName}'");

					if (!OnEnterDir(fiDir)) return;
					if (_cts.IsCancellationRequested) return;


					FileSystemInfo[] aChildrens = Array.Empty<FileSystemInfo>();
					try { aChildrens = fiDir.GetFileSystemInfos(); }
					catch (Exception exGetFiles)
					{
						var ERR = new SCAN_ERROR(fiDir, exGetFiles, ERROR_SOURCES.FAILED_GET_FOLDER_CONTENT);
						if (!OnError(ERR)) return;
					}


					try
					{
						var aFiles = aChildrens.Where(fsi => fsi.e_IsFileInfo()).Cast<FileInfo>().ToArray();
						foreach (var F in aFiles)
						{
							if (!OnFileFound(F)) break;
						}
					}
					catch (Exception exGetFiles)
					{
						var ERR = new SCAN_ERROR(fiDir, exGetFiles, ERROR_SOURCES.Unknown);
						if (!OnError(ERR)) return;
					}

					if (_cts.IsCancellationRequested) return;

					try
					{
						var aDirs = aChildrens.Where(fsi => fsi.e_IsDirectory()).Cast<DirectoryInfo>().ToArray();
						aDirs = OnBeforeCheckSubDirs(aDirs);
						if (null == aDirs || !aDirs.Any()) return;

						ScanDirs(aDirs);
					}
					catch (Exception exGetDirs)
					{
						var ERR = new SCAN_ERROR(fiDir, exGetDirs, ERROR_SOURCES.Unknown);
						if (!OnError(ERR)) return;
					}


				}
				finally
				{
					ThreadsCounter.Decrement();
				}
			}

			/// <summary>Occurs when SCANNER enters to found directory</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnEnterDir(DirectoryInfo D) => (!_cts.IsCancellationRequested);

			/// <summary>Occurs before SCANNER begins scan subdirectories</summary>
			/// <returns>Tou can modify array of subDirs to scan, or return null to stop</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual DirectoryInfo[] OnBeforeCheckSubDirs(DirectoryInfo[] aDirs) => aDirs;

			/// <summary>Occurs when SCANNER found any file</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnFileFound(FileInfo F) => (!_cts.IsCancellationRequested);


			/// <summary>Periodicaly Occurs when some tasks started and finished
			/// Just used for write any messages showinf that app is not hang</summary>            
			protected virtual void OnWaitTasks(SCAN_RUNNING_INFO e)
			{
				//Just waste some time to finish some tasks
				Thread.Sleep(500);
			}

			/// <summary>Occurs when SCANNER found any ERROR</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnError(SCAN_ERROR e) => (!_cts.IsCancellationRequested);
		}

		/// <summary>Multithread File Searcher</summary>
		internal class FileSearcher : uom.I_O.FileSystemScannerBase
		{
			public FileSearcher(int iParralelTasks = 20) : base() { }

			private readonly List<FileInfo> _lResult = new(1000);
			private string[] _aFilesToFind = Array.Empty<string>();


			private Func<FileInfo, bool>? _cbOnFileFound = null;
			private Func<DirectoryInfo, bool>? _cbOnOnEnterDir = null;
			private Func<DirectoryInfo[], DirectoryInfo[]>? _cbOnBeforeCheckSubDirs = null;
			private Func<SCAN_ERROR, bool>? _cbOnError = null;
			private Action<SCAN_RUNNING_INFO>? _cbOnWaitTasks = null;

			public FileInfo[] SearchFiles(
				string[] aFilesToFind,
				DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)
			{
				_ = aFilesToFind ?? throw new ArgumentNullException(nameof(aFilesToFind));
				_aFilesToFind = aFilesToFind.Where((f) => f.e_IsNOTNullOrWhiteSpace()).ToArray();
				if (!_aFilesToFind.Any()) throw new ArgumentNullException(nameof(aFilesToFind));

				_cbOnOnEnterDir = fuOnEnterDir;
				_cbOnBeforeCheckSubDirs = fuOnBeforeCheckSubDirs;
				_cbOnFileFound = fuFileFound;
				_cbOnError = fuOnError;
				_cbOnWaitTasks = OnWaitTasks;

				_lResult.Clear();
				base.Scan(aRootDirsToScan, maxDegreeOfParallelism);

				var aFiles = (from L in _lResult orderby L.FullName select L).ToArray();
				return aFiles;
			}


			protected override bool OnEnterDir(DirectoryInfo D)
			{
				if (!base.OnEnterDir(D)) return false;
				var bContinueToSearch = _cbOnOnEnterDir?.Invoke(D);
				return bContinueToSearch.e_ToBool();
			}

			protected override DirectoryInfo[] OnBeforeCheckSubDirs(DirectoryInfo[] aDirs) =>
				(_cbOnBeforeCheckSubDirs == null) ? base.OnBeforeCheckSubDirs(aDirs) : _cbOnBeforeCheckSubDirs.Invoke(aDirs);

			protected override bool OnFileFound(FileInfo F)
			{
				if (!base.OnFileFound(F)) return false;
				var aFound = _aFilesToFind.Where((s) => s.ToLower().Equals(F.Name.ToLower()));
				if (!aFound.Any()) return true;

				_lResult.Add(F);

				var bContinueToSearch = _cbOnFileFound?.Invoke(F);
				return bContinueToSearch.e_ToBool();
			}

			protected override bool OnError(SCAN_ERROR e)
			{
				return (_cbOnError == null) ? base.OnError(e) : _cbOnError.Invoke(e);
			}

			protected override void OnWaitTasks(SCAN_RUNNING_INFO e)
			{
				if (null == _cbOnWaitTasks) { base.OnWaitTasks(e); return; }
				_cbOnWaitTasks?.Invoke(e);
			}






			public static async Task<FileInfo[]> SearchFileAsync(
				string[] aFilesToFind,
				DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)

			{
				var FS = new FileSearcher();
				using (var tskSearch = new Task<FileInfo[]>(
					() => FS.SearchFiles(
						aFilesToFind,
						aRootDirsToScan,
						fuOnEnterDir,
						fuOnBeforeCheckSubDirs,
						fuFileFound,
						fuOnError,
						OnWaitTasks,
						maxDegreeOfParallelism
					), TaskCreationOptions.LongRunning))
				{
					tskSearch.Start();
					var aFound = await tskSearch;
					return aFound;
				};
			}

			public static async Task<FileInfo[]> SearchFileAsync(
			   string sFileToFind,
			   DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)
			{
				var aFiles = new string[] { sFileToFind };
				return await SearchFileAsync(
					aFiles,
					aRootDirsToScan,
					fuOnEnterDir,
					fuOnBeforeCheckSubDirs,
					fuFileFound,
					fuOnError,
					OnWaitTasks,
					maxDegreeOfParallelism);
			}
		}


		/// <summary>Reads file lines in background thread.
		/// Usable fo read any log files</summary>
		internal class BackgroundLogFileReader
		{
			public event EventHandler<string> LineRead = delegate { };
			public event EventHandler<Exception> IOError = delegate { };

			private FileInfo? _File = null;
			private StreamReader? _SR = null;

			public MTSafeBooleanFlag StopFlag { get; private set; } = new MTSafeBooleanFlag(false);
			private Thread? _thRead = null;
			private readonly ManualResetEvent _evtFinished = new(false);

			public bool DeleteFileOnFinish { get; private set; } = false;



			public BackgroundLogFileReader(string sPath, bool bDeleteFileOnFinish = false) : base()
			{
				DeleteFileOnFinish = bDeleteFileOnFinish;
				_File = sPath.e_ToFileInfo();
				_SR = _File!.e_CreateReader();
				StartCore(sPath);
			}
			public BackgroundLogFileReader(Stream S) : base()
			{
				DeleteFileOnFinish = false;
				_SR = S.e_CreateReader();
				StartCore("[Stream]");
			}

			private void StartCore(string ThreadName)
			{
				_thRead = new Thread(MainReadThread);
				{
					_thRead.IsBackground = true;
					_thRead.Name = $"Reading '{ThreadName}' lines in background thread...";
					_thRead.Start();
				}
			}

			private bool IsFile => (_File != null);

			private void MainReadThread()
			{


				try
				{
					ReadStream();
				}
				catch (Exception ex)
				{
					IOError?.Invoke(this, ex);
				}
				finally
				{
					if (IsFile)
					{
						try { _SR!.Dispose(); } catch { }
						if (DeleteFileOnFinish)
							try { _File!.Delete(); } catch { }
						_SR = null;
						_File = null;
					}
					_evtFinished.Set();
				}
			}


			private void ReadStream()
			{
				while (StopFlag.IsSet == false)
				{
					try
					{
						string? sLine = _SR!.ReadLine();
						if (null != sLine) LineRead?.Invoke(this, sLine!);
						else Thread.Sleep(100);
					}
					catch { }// Just Ignore error on read single line and try read next line
				}
			}

			public void Stop(int iTimeout = System.Threading.Timeout.Infinite)
			{
				StopFlag.SetlFlag();
				_evtFinished.WaitOne(iTimeout);
			}
		}
	}


	internal static class Net
	{
		//[DebuggerNonUserCode, DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInDomain()
		{
			try
			{
				//_ = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
				return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName.e_IsNOTNullOrWhiteSpace();
			}
			catch { return false; }
		}

		//https://newbedev.com/progress-bar-with-httpclient
		public class HttpClientDownloadWithProgress : IDisposable
		{
			private readonly string _downloadUrl;
			private readonly string _destinationFilePath;

			private HttpClient? _httpClient = null;

			public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);
			public event ProgressChangedHandler ProgressChanged = delegate { };
			//public event EventHandler<string> LineRead = delegate { };

			public HttpClientDownloadWithProgress(string downloadUrl, string destinationFilePath)
			{
				_downloadUrl = downloadUrl;
				_destinationFilePath = destinationFilePath;
			}

			public async Task StartDownload()
			{
				_httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

				using var response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
				await DownloadFileFromHttpResponseMessage(response);
			}

			private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
			{
				response.EnsureSuccessStatusCode();
				var totalBytes = response.Content.Headers.ContentLength;
				using var contentStream = await response.Content.ReadAsStreamAsync();
				await ProcessContentStream(totalBytes, contentStream);
			}

			private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
			{
				var totalBytesRead = 0L;
				var readCount = 0L;
				var buffer = new byte[8192];
				var isMoreToRead = true;

				using FileStream fileStream = new(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
				do
				{
					int bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
					if (bytesRead == 0)
					{
						isMoreToRead = false;
						TriggerProgressChanged(totalDownloadSize, totalBytesRead);
						continue;
					}

					await fileStream.WriteAsync(buffer, 0, bytesRead);

					totalBytesRead += bytesRead;
					readCount += 1;

					if (readCount % 100 == 0)
						TriggerProgressChanged(totalDownloadSize, totalBytesRead);
				}
				while (isMoreToRead);
			}

			private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
			{
				if (ProgressChanged == null)
					return;

				double? progressPercentage = null;
				if (totalDownloadSize.HasValue)
					progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

				ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
			}

			public void Dispose()
			{
				_httpClient?.Dispose();
			}
		}
	}


	internal static class Secur
	{
		/// <summary>An implementation of System.Random that uses RNGCryptoServiceProvider to provide random values.</summary>
		internal class CryptoRandom : Random, IDisposable
		{
			private readonly RandomNumberGenerator _csp = RandomNumberGenerator.Create();

			#region IDisposable Support
			private bool disposedValue; // To detect redundant calls
										// This code added by Visual Basic to correctly implement the disposable pattern.
			public void Dispose()
			{
				if (!disposedValue) _csp.Dispose();
				disposedValue = true;
				// TODO: uncomment the following line if Finalize() is overridden above.
				// GC.SuppressFinalize(Me)
			}
			#endregion

			/// <summary> Returns a random number between [0.0 And 1.0). </summary>
			protected override double Sample()
			{
				var abUInt64 = Random_Bytes(_csp, 8);
				long value = BitConverter.ToInt64(abUInt64, 0) & long.MaxValue;
				// Scale it to 0->1
				return value / (long.MaxValue + 1025d);
			}

			/// <summary>Random number in range from [minValue] to [maxValue]. All limits are included</summary>
			public override int Next(int minValue, int maxValue)
			{
				if (minValue == maxValue) return minValue;
				var range = (double)(maxValue - minValue);
				var NewSample = Sample();
				int iVal = (int)Math.Round(NewSample * range) + minValue;
				if (iVal > maxValue) iVal = maxValue;
				return iVal;
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static byte[] Random_Bytes(RandomNumberGenerator rng, int iLen)
			{
				var abRandom = new byte[iLen];
				rng.GetBytes(abRandom);
				return abRandom;
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static byte[] Random_Bytes(int iLen)
			{
				using var RNG = RandomNumberGenerator.Create();
				return Random_Bytes(RNG, iLen);
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static byte Random_Byte() => Random_Bytes(1)[0];

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static UInt16 Random_UInt16() => BitConverter.ToUInt16(Random_Bytes(sizeof(UInt16)), 0);

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static UInt32 Random_UInt32() => BitConverter.ToUInt32(Random_Bytes(sizeof(UInt32)), 0);

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static UInt64 Random_UInt64() => BitConverter.ToUInt64(Random_Bytes(sizeof(UInt64)), 0);
		}

		/// <summary>
		/// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.create?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Security.Cryptography.HashAlgorithm.Create);k(DevLang-csharp)%26rd%3Dtrue&view=net-6.0
		/// </summary>
		internal enum HashNames : int
		{
			SHA1,
			MD5,
			SHA256,
			SHA384,
			SHA512
		}
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static HashAlgorithm? CreateHashAlgorithm(HashNames hn) => HashAlgorithm.Create(hn.ToString());

		/// <summary>
		/// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.keyedhashalgorithm.create?view=netframework-4.8&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Security.Cryptography.KeyedHashAlgorithm.Create);k(TargetFrameworkMoniker-.NETFramework,Version%253Dv4.8);k(DevLang-csharp)%26rd%3Dtrue
		/// </summary>
		internal enum KeyedHashNames : int
		{
			HMACSHA1,
			HMACMD5,
			HMACRIPEMD160,
			HMACSHA256,
			HMACSHA384,
			HMACSHA512,
			MACTripleDES
		}
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static KeyedHashAlgorithm? CreateKeyedHashAlgorithm(KeyedHashNames kha) => KeyedHashAlgorithm.Create(kha.ToString());

	}



#pragma warning disable IDE1006 // Naming Styles

	namespace Extensions
	{

		/// <summary>Numeric Extensions</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_Numeric
		{

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T IIF<T>(this bool Expression, T TrueResult, T FalseResult) => Expression ? TrueResult : FalseResult;



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_ToBool(this bool? bExpression) => (null != bExpression && bExpression.HasValue) && bExpression.Value;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_ToBool(this bool bExpression) => bExpression;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_ToBool(this int iValue) => (0 != iValue);









			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int e_ToInt32ABS(this bool bValue) => (Int32)(bValue ? 1 : 0);











			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int[] e_RangeTo(this int iFrom, int iTo) => Enumerable.Range(iFrom, iTo).ToArray();








			/// <summary>Чётное</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsEven(this int N) => (N % 2 == 0);


			/// <summary>Нечётное</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsOdd(this int N) => (!N.e_IsEven());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsКратно(this int Value, int ЧемуКратно)
				=> (Value / (double)ЧемуКратно == Value / ЧемуКратно);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsКратно(this long Value, long ЧемуКратно)
				=> (Value / (double)ЧемуКратно == Value / ЧемуКратно);


			#region CheckRange

			/*             
            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int e_CheckRange(this int value, int minValue = 0, int maxValue = 100)
            {
                if (value < minValue) value = minValue;
                else if (value > maxValue) value = maxValue;
                return value;
            }

            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Int64 e_CheckRange(this Int64 value, Int64 minValue = 0, Int64 maxValue = 100)
            {
                if (value < minValue) value = minValue;
                else if (value > maxValue) value = maxValue;
                return value;
            }

            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float e_CheckRange(this float value, float minValue = 0, float maxValue = 100)
            {
                if (value < minValue) value = minValue;
                else if (value > maxValue) value = maxValue;
                return value;
            }
            */

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_CheckRange<T>(this T Value, T? MinLimit = default, T? MaxLimit = default) where T : struct, IComparable
			{
				try
				{
					if (null != MinLimit && MinLimit.HasValue && Value.CompareTo(MinLimit.Value) < 0)
						Value = MinLimit.Value;

					else if (null != MaxLimit && MaxLimit.HasValue && Value.CompareTo(MaxLimit.Value) > 0)
						Value = MaxLimit.Value;

					return Value;
				}
				catch
				{
					return MinLimit.e_ValueOrNull() ?? default;
				}
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T? e_ValueOrNull<T>(this T? v) where T : struct
				=> (null != v && v.HasValue) ? v.Value : null;

			#endregion



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float e_Round(this float N, int Precision) => (float)Math.Round(N, Precision);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static double e_Round(this double N, int Precision) => Math.Round(N, Precision);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static decimal e_Round(this decimal N, int Precision) => Math.Round(N, Precision);












			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int64 e_ToInt64(this long V) => new(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int64 e_ToInt64(this ulong V) => new(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int32 e_ToInt32(this int V) => new(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int32 e_ToInt32(this uint V) => new(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int16 e_ToInt16(this short V) => new(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uom._Int16 e_ToInt16(this ushort V) => new(V);











		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_Binary_Hex
		{

			#region HIWORD/LOWORD/MAKELPARAM

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint e_MAKELPARAM(this ushort LoWord16, ushort HiWord16) => (uint)((uint)HiWord16 << 16 | LoWord16 & 0xFFFFL);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint e_MakeLong(this ushort LoWord16, ushort HiWord16) => LoWord16.e_MAKELPARAM(HiWord16);


			//e_HiWord
			//if ((UI32 & 0x80000000U) == 0x80000000U) return (ushort)(UI32 >> 16);
			//else return (ushort)(UI32 >> 16 & 0xFFFFu);
			//internal static ushort e_LoWord(this uint UI32) => (ushort)(UI32 & 0xFFFFL);
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ushort e_LoWord(this uint i) => new _Int32(i).ULoWord;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static short e_LoWord(this int i) => new _Int32(i).LoWord;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ushort e_HiWord(this uint i) => new _Int32(i).UHiWord;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static short e_HiWord(this int i) => new _Int32(i).HiWord;



			#endregion



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static long e_MakeFourCC(this byte ch0, byte ch1, byte ch2, byte ch3)
			{
				throw new NotImplementedException();
				// Dim lRes&, lVal&
				// lRes = ch0

				// lVal = ch1
				// lVal = lVal * (2 ^ 8) : lRes = lRes Or lVal

				// lVal = ch2
				// lVal = lVal * (2 ^ 16) : lRes = lRes Or lVal

				// lVal = ch3
				// Dim A&
				// A = 2 ^ 24
				// MsgBox A, , Hex(A)
				// lVal = lVal * (2 ^ 24)
				// lRes = lRes Or lVal

				// Return lRes
			}



			private const string C_BAD_BYTE_SEPARATOR_CHARS = @" :_|./*+,\~`'=";
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_NormalizeHexString(this string HexString, string? BadSeparators = null, char? GoodHexSeparator = null)
			{
				if (HexString.e_IsNullOrWhiteSpace()) return HexString;

				BadSeparators ??= C_BAD_BYTE_SEPARATOR_CHARS;
				GoodHexSeparator ??= constants.SystemDefaultHexByteSeparator;

				return HexString.Trim().ToUpper().e_ReplaceAll2(BadSeparators.ToCharArray().e_ToStringArray(), GoodHexSeparator!.ToString()!);
			}


			/// <summary>Получаем массив байт из строки</summary>
			/// <param name="HexString">Строка вида: 0D-0A-2B / 43:53:51:3A / 43.53.51.3A / </param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_HexStringToBytes(this string HexString, string ByteSeparatorChars = C_BAD_BYTE_SEPARATOR_CHARS)
			{
				if (HexString.e_IsNullOrWhiteSpace()) return Array.Empty<byte>();

				var SEPARATOR_CHAR = constants.SystemDefaultHexByteSeparator;
				HexString = HexString.e_NormalizeHexString(ByteSeparatorChars, SEPARATOR_CHAR);
				var abResult = Array.Empty<byte>();
				var asHexBytes = HexString.Trim().Split(SEPARATOR_CHAR); // Делим по разделителю на отдельные элементы
				if (asHexBytes.Any())
				{
					try
					{
						var aBytes = (from sHexByte in asHexBytes
									  let bByte = byte.Parse(sHexByte, NumberStyles.HexNumber)
									  select bByte).ToArray();
						abResult = aBytes;
					}
					catch (Exception ex) { string S = ex.Message; }
				}
				return abResult;
			}




			#region BitArray / Bits

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this long V) => BitConverter.GetBytes(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this ulong V) => BitConverter.GetBytes(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this int V) => BitConverter.GetBytes(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this uint V) => BitConverter.GetBytes(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this short V) => BitConverter.GetBytes(V);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this ushort V) => BitConverter.GetBytes(V);


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this BitArray BA)
			{
				var iBytesCount = (int)Math.Round(Math.Ceiling(BA.Count / 8f));
				var abMaxIP = new byte[iBytesCount];
				BA.CopyTo(abMaxIP, 0);
				return abMaxIP;
			}


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes(this IEnumerable<bool> aBits) => (new BitArray(aBits.ToArray())).e_GetBytes();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this byte V) => new(new byte[] { V });


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this long V) => new(V.e_GetBytes());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this int V) => new(V.e_GetBytes());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this ulong V) => new(V.e_GetBytes());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this uint V) => new(V.e_GetBytes());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this ushort V) => new(V.e_GetBytes());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static BitArray e_GetBits(this short V) => new(V.e_GetBytes());


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this BitArray BA) => BA.Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this int V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this long V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this ulong V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this uint V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this ushort V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this short V) => V.e_GetBits().Cast<bool>().ToArray();


			/// <summary>Возвращает массив битов, младший бит вначале (0x1 = '10000000')</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool[] e_GetBitsAsBool(this byte V, bool bReverse = false)
			{
				var aBits = V.e_GetBits().Cast<bool>().ToArray();
				return (bReverse) ? aBits.Reverse().ToArray() : aBits;
			}


			private const string C_ERROR_NULL_MASK = "Bit mask must be >0!";
			/// <summary>Установлены ли все биты по заданной маске</summary>
			/// <param name="SourceValue">Значение, в котором проверяются биты</param>
			/// <param name="BitMask">Маска, по которой проверяются биты</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsBitsSetByMask(this int SourceValue, int BitMask)
			{
				if (BitMask <= 0) throw new ArgumentOutOfRangeException(C_ERROR_NULL_MASK);
				return (SourceValue & BitMask) == BitMask;
			}


			/// <summary>Установлен ли бит по заданному индексу (индексы от 0)</summary>
			/// <param name="SourceValue">Значение, в котором проверяются биты</param>
			/// <param name="iZeroBasetBitIndexToCheck">Номер бита (индексы от 0)</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsBitSetByIndex(this int SourceValue, int iZeroBasetBitIndexToCheck)
			{
				int BitMask = (int)Math.Round(Math.Pow(2d, iZeroBasetBitIndexToCheck));
				return (SourceValue & BitMask) == BitMask; ;
			}


			/// <summary>Установлен ли бит по заданному индексу (индексы от 0)</summary>
			/// <param name="SourceValue">Значение, в котором проверяются биты</param>
			/// <param name="iZeroBasetBitIndexToCheck">Номер бита (индексы от 0)</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsBitSetByIndex(this long SourceValue, int iZeroBasetBitIndexToCheck)
			{
				long BitMask = (long)Math.Round(Math.Pow(2d, iZeroBasetBitIndexToCheck));
				return (SourceValue & BitMask) == BitMask;
			}


			/// <summary>Включает или выключает биты по маске</summary>
			/// <param name="SourceValue">Значение, в котором включаются/выключаются биты</param>
			/// <param name="BitMask">Маска, по которой включаются/выключаются биты</param>
			/// <param name="bSet">Вкл / выкл</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_SetBitsByMask(this int SourceValue, int BitMask, bool bSet = true)
			{
				if (BitMask <= 0)
					throw new ArgumentOutOfRangeException(C_ERROR_NULL_MASK);
				if (bSet)
				{
					// Устанавливаем биты по маске
					return SourceValue | BitMask;
				}
				else
				{
					// Если биты по маске не установлены, то выходим
					if (!SourceValue.e_IsBitsSetByMask(BitMask)) return SourceValue;

					// Снимаем биты по маске
					return (SourceValue ^ BitMask) & SourceValue;
					// Return (SourceValue And (Not SetMask))
				}
			}


			/// <summary>Включает или выключает бит по заданному индексу (индексы от 0)</summary>
			/// <param name="SourceValue">Значение, в котором проверяются биты</param>
			/// <param name="iZeroBasetBitIndex">Номер бита для включения/выключения (индексы от 0)</param>
			/// <param name="bSet">Вкл / выкл</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_SetBitByIndex(this int SourceValue, int iZeroBasetBitIndex, bool bSet = true)
			{
				int BitMask = (int)Math.Round(Math.Pow(2d, iZeroBasetBitIndex));
				return SourceValue.e_SetBitsByMask(BitMask, bSet);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_IsBitSetByIndexWithOffset(this int SourceValue, int iZeroBasedStartBitIndex, int iBitCountToCheck)
			{
				SourceValue >>= iZeroBasedStartBitIndex;
				uint iMax = uint.MaxValue;
				int iTotalBits = Marshal.SizeOf(iMax) * 8;
				int iShift = iTotalBits - iBitCountToCheck;
				int iMask = (int)(iMax >> iShift);
				return SourceValue & iMask;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_SetBitByMaskWithOffset(this int SourceValue, int iZeroBasedStartBitIndex, int iBitCountAtValue, int iValueToSet)
			{
				iValueToSet <<= iZeroBasedStartBitIndex;
				uint iMax = uint.MaxValue;
				int iTotalBits = Marshal.SizeOf(iMax) * 8;
				int lShift = iTotalBits - iBitCountAtValue;

				// Маска для выключения битов, на место которых будет вставляться нужное значение
				int iMask = (int)(iMax >> lShift);
				iMask <<= iZeroBasedStartBitIndex;

				// Выключаем биты под вставку
				SourceValue = SourceValue.e_SetBitsByMask(iMask, false);
				int iResult = SourceValue | iValueToSet;
				return iResult;
			}





			#region В строку битов  вида 0000-0001
			/// <summary>Возвращает строку битов вида 0000-0001</summary>
			/// <param name="iGroupSize">Размер октета, для расстановки отступов</param>
			/// <param name="bReverseRTL">Разверноуть в программисткий вид (младший байт будет справа)</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringOfBits(this BitArray BA, char Char_0 = '0', char Char_1 = '1', int iGroupSize = 8, bool bReverseRTL = true)
			{
				var aBoolValues = BA.e_GetBitsAsBool();
				var lBits = new List<char>(aBoolValues.Length * 2);
				int iPos = 0;
				foreach (var B in aBoolValues)
				{
					iPos += 1;
					char cBit = B ? Char_1 : Char_0;
					lBits.Add(cBit);
					if (iPos >= iGroupSize)
					{
						lBits.Add(' ');
						iPos = 0;
					}
				}

				var aBits = lBits.ToArray();
				if (bReverseRTL)
					aBits = aBits.Reverse().ToArray();
				string sBits = new(aBits);
				return sBits.Trim();
			}
			/// <summary>Возвращает строку битов вида 0000-0001</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringOfBits(this int iVal) => iVal.e_GetBits().e_ToStringOfBits();


			/// <summary>Возвращает строку битов вида 0000-0001</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringOfBits(this long iVal) => iVal.e_GetBits().e_ToStringOfBits();


			#endregion

			#endregion





			#region HexDump
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes_Default(this string sData) => Encoding.Default.GetBytes(sData);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes_ASCII(this string sData) => Encoding.ASCII.GetBytes(sData);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetBytes_Unicode(this string sData) => Encoding.Unicode.GetBytes(sData);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_GetBytes_Unicode_ToBase64String(this string sData) => Convert.ToBase64String(sData.e_GetBytes_Unicode());









			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string HexDump(this IntPtr lpBuffer, int nBytes)
			{
				//var sPrefix = $"Mem Buffer, Ptr=0x{lpBuffer.ToInt64().ToString("X")}, (DEC){lpBuffer.ToInt64().ToString()})";
				var abData = lpBuffer.e_PtrToBytes(nBytes);
				return abData.HexDump(lpBuffer.ToInt64());
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string HexDump(this byte[] abData, long StartAddress = long.MinValue)
			{
				const int SPLIT_LENGHT = 16;
				var sbResult = new StringBuilder(); // (dBytes + 100)
				int iCopyLenght = SPLIT_LENGHT;
				var abFullLine = new string('-', SPLIT_LENGHT).e_GetBytes_ASCII();
				int iFullLineLenght = BitConverter.ToString(abFullLine, 0, abFullLine.Length).Length;
				long lLineAddress = StartAddress;
				for (int iPos = 0, loopTo = abData.Length - 1; iPos <= loopTo; iPos += SPLIT_LENGHT)
				{
					int iNextPos = iPos + iCopyLenght;
					if (iNextPos > abData.Length - 1)
						iCopyLenght = abData.Length - iPos;
					var abString = new byte[iCopyLenght];
					Array.Copy(abData, iPos, abString, 0, iCopyLenght);
					string sLine = BitConverter.ToString(abString, 0, iCopyLenght);
					sLine = sLine.PadRight(iFullLineLenght, '-');
					if (StartAddress != long.MinValue)
					{
						// Надо добавить адрес строки
						sbResult.Append(lLineAddress.ToString("X").PadLeft(Marshal.SizeOf(lLineAddress) * 2, '0'));
						sbResult.Append('|');
					}

					sbResult.Append(sLine);
					sbResult.Append('|');
					foreach (byte bChar in abString)
					{
						switch (bChar)
						{
							case > 31: sbResult.Append(bChar.e_ToChar()); break;
							default: sbResult.Append('.'); ; break;
						}
					}

					sbResult.AppendLine();
					lLineAddress += SPLIT_LENGHT;
				}

				return sbResult.ToString();
			}

			#endregion







		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_Math_Conversions
		{


			internal const ulong C_BYTES_IN_KBYTE = 1024UL;
			internal const ulong C_BYTES_IN_MBYTE = 1048576UL;
			internal const ulong C_BYTES_IN_GBYTE = 1073741824UL;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ulong e_KBToBytes(this int MB) => (ulong)MB * C_BYTES_IN_KBYTE;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ulong e_MBToBytes(this int MB) => (ulong)MB * C_BYTES_IN_MBYTE;







			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float e_InchesToMM(this float Inches) => Inches * constants.C_MM_IN_INCH;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float e_MMToInches(this float MM) => MM / constants.C_MM_IN_INCH;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float e_InchesToCM(this float Inches) => Inches * constants.C_CM_IN_INCH;




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float e_CMToInches(this float CM) => CM / constants.C_CM_IN_INCH;














		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_StringAndFormat
		{



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToNonNull(this string? SourceText) => (SourceText ?? "");


			/// <summary>(SourceText Is Nothing) OrElse (SourceText.Length 1)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNullOrEmpty(this string? SourceText) => ((null == SourceText) || (SourceText!.Length < 1));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNullOrWhiteSpace(this string? SourceText) => ((null == SourceText) || string.IsNullOrWhiteSpace(SourceText!));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNOTNullOrWhiteSpace(this string? SourceText) => (!SourceText.e_IsNullOrWhiteSpace());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNOTNullOrWhiteSpaceAndStartsWith(this string? SourceText, string sFindWhat) => SourceText.e_IsNOTNullOrWhiteSpace() && SourceText!.StartsWith(sFindWhat);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNOTNullOrWhiteSpaceAndEndsWith(this string? SourceText, string sFindWhat) => SourceText.e_IsNOTNullOrWhiteSpace() && SourceText!.EndsWith(sFindWhat);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string IIF_IsNullOrWhiteSpace(this string? SourceText, string NullValueString = "")
				=> (SourceText.e_IsNullOrWhiteSpace()) ? NullValueString : SourceText!;


			/// <summary>The strings is differ only in case?</summary>
			internal static bool e_IsDifferOlyInCase(this string? s1, string? s2)
			{
				if (string.Compare(s1, s2) == 0) return false;
				if (null == s1) return false;
				if (null == s2) return false;

				return ((s1.ToLower() == s2.ToLower()) && (s1 != s2));
			}






















			/// <summary>Соединяет массив строк в одну строку с заданными разделителями</summary>
			/// <param name="eStrings">Массив строк для объединения</param>
			/// <param name="Separator">Разделитель, который будет добавлен в конце каждой строки</param>
			/// <returns>Строку в которую соединены все строки</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string? e_Join(
				this IEnumerable<string>? eStrings,
				string Separator = " ",
				string? EmptyOrNothingValue = null)
				=> ((null == eStrings) || !eStrings!.Any()) ? EmptyOrNothingValue : string.Join(Separator, eStrings);




			/// <summary>Соединяет массив строк в одну строку с заданными разделителями</summary>
			/// <param name="Separator">Разделитель</param>
			/// <returns>Строку в которую соединены все строки</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Join(
				this System.Collections.Specialized.StringCollection? cSpecializedStringCollection,
				string Separator = " ")
				=> (null == cSpecializedStringCollection) ? "" : cSpecializedStringCollection!.Cast<string>().e_Join(Separator)!;





			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Repeat(this char cChar, int Length = 70) => new(cChar, Length);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int e_WordCount(this string str) => str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Indent(this string originalString, int indentLevel = 1, Char indentChar = '\t')
			{
				var aLines = originalString.Split('\n');
				var sb = new StringBuilder();
				foreach (var s in aLines) sb.AppendLine((new string(indentChar, indentLevel) + s));
				return sb.ToString().TrimEnd();

				/*
        var indentedString = new StringBuilder();
        indentedString.Append("".PadLeft(indentLevel * _indentSize));
        indentedString.Append(originalString);
        return indentedString.ToString();
                 */
			}





			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToStringAllowNull(
				this string? source,
				string nullValue = "null",
				string emptyValue = "''")
			{
				if (source == null) return nullValue;
				if (string.IsNullOrEmpty(source)) return emptyValue;
				return source;
			}


			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Format(this string sFormatString, params object[] Args) => string.Format(sFormatString, Args);


			private static readonly string[] ByteSize_En = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
			private static readonly string[] ByteSize_Ru = { "Б", "КБ", "МБ", "ГБ", "ТБ", "ПБ", "ЕБ" };
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FormatByteSize(this Int64 BytesLength, int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS)
			{
				string[] sizes = uom.AppInfo.CurrentUICultureIsRuTree ? ByteSize_Ru : ByteSize_En;
				double dblLen = (double)BytesLength;
				int order = 0;
				while (dblLen >= 1024 && (order < sizes.Length - 1))
				{
					order++;
					dblLen /= 1024;
				}
				// Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a single decimal place, and no space.
				string sFormat = (iDecimalPlaces > 0) ? ("{0:0." + new String('#', iDecimalPlaces) + "} {1}") : "{0:0} {1}";
				var result = string.Format(sFormat, dblLen, sizes[order]);
				return result;
			}



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FormatByteSize(
				this int BytesLength,
				int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS) => ((Int64)BytesLength).e_FormatByteSize(iDecimalPlaces);























			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FormatPercent(this int iPercentValue, bool RightAlign = true)
				=> ((float)((float)iPercentValue.e_CheckRange() / (float)100)).e_FormatPercent(0, RightAlign);

			/// <summary>Возвращает строку вида '20,23 %'</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FormatPercent(this float fPercentValue, int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS, bool RightAlign = true)
			{
				if (float.IsNaN(fPercentValue)) fPercentValue = 0F;
				var sFormat = $"P{iDecimalPlaces}";
				var sValue = fPercentValue.ToString(sFormat);
				if (RightAlign)
				{
					var sPercent100 = ((float)1).ToString(sFormat);
					sValue = sValue.PadLeft(sPercent100.Length, ' ');
				}
				return sValue;
			}


			/// <summary>Возвращает строку вида '20,2%'</summary>
			/// <param name="PercentValue">Значение от 0,0 до 1,0 !!!НЕ от 0 до 100!!!</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FormatPercent(this double PercentValue, int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS)
			{
				if (double.IsNaN(PercentValue)) PercentValue = 0d;
				return ((float)PercentValue).e_FormatPercent(iDecimalPlaces);
			}










			/// <summary>Format number like '1 000 000'</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Format(this int iValue) => iValue.ToString("N0").Trim();


			/// <summary>Выводит число как строку, разделяя тысячные разряды пробелом </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Format2(this int Value)
			{
				return Value.ToString(constants.C_FMT_LONGNUMBER).Trim();
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Format(this long iValue) => iValue.ToString("N0").Trim();



			[Obsolete("!!!Need to Verify correct output!!!", true)]
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Format(this float fValue, int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS)
				=> fValue.ToString($"N{iDecimalPlaces}").Trim();

			/// <summary>Выводит число как строку, разделяя тысячные разряды пробелом </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Format2(this float Value, int Prcission = 3)
			{
				string sFormat = constants.C_FMT_LONGNUMBER + "." + "".PadRight(Prcission, '0');
				return Value.ToString(sFormat).Trim();
			}





			/// <summary>Выводит число как строку, разделяя тысячные разряды пробелом </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Format2(this long Value)
				=> Value.ToString(constants.C_FMT_LONGNUMBER).Trim();



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Format(this IntPtr Value)
				=> e_Format(Value.ToInt64());










			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Format_PlusMinus(this bool bValue) => bValue ? "+" : "-";

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_Format_YesNoGlobal(this bool bValue)
			{
				if (uom.AppInfo.CurrentUICultureIsRuTree)
					return bValue ? constants.C_YES_RUS : constants.C_NO_RUS;
				else
					return bValue ? constants.C_YES_ENG : constants.C_NO_ENG;
			}



			/// <summary>iProgress из iMax (20.25%)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_FormatProgress(this int iProgress, int iMax, int PercentDigits = 2)
			{
				float sngProgress = 0f;
				if (iMax > 0) sngProgress = iProgress / (float)iMax;
				return "{0} из {1} ({2})".e_Format(iProgress, iMax, sngProgress.e_FormatPercent(PercentDigits));
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_FormatProgressConsole(this int iACurrent, int iBTotal, string sFormat = "G")
			{
				string sBTotal = iBTotal.ToString(sFormat);
				string sACurrent = iACurrent.ToString(sFormat).PadLeft(sBTotal.Length, ' ');
				return "{0} из {1}".e_Format(sACurrent, sBTotal);
			}






			/// <summary>Возвращает строку из байт, заменяя нечинаемые символы заменителями</summary>
			/// <param name="abData">Массив байт</param>
			/// <param name="cNotreadableCharsReplacement">Заменитель нечитаемых символов</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static char e_ToChar(this byte B) => Convert.ToChar(B);
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static char[] e_ToChars(this IEnumerable<byte> abData, char? cNotreadableCharsReplacement = null)
				=> abData.Select<byte, char>((B) => ((B < 32) && (null != cNotreadableCharsReplacement)) ? (char)cNotreadableCharsReplacement! : B.e_ToChar()).ToArray();


			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static long  e_UnHex(this string HexVal) => (long)Math.Round(Conversion.Val("&H" + HexVal));

			/// <summary>Возвращает строку из байт, заменяя нечинаемые символы заменителями</summary>
			/// <param name="abData">Массив байт</param>
			/// <param name="cNotreadableCharsReplacement">Заменитель нечитаемых символов</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringAnsiASC(this IEnumerable<byte>? abData, char? cNotreadableCharsReplacement = '.')
				=> (abData == null || !abData.Any()) ? "" : new string(abData!.e_ToChars(cNotreadableCharsReplacement));


			/// <summary>Возвращает строку из байт, включая нечитаемые символы (используется Text.Encoding.ASCII.GetString)</summary>
			/// <param name="abData">Массив байт</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringAnsiASCFast(this IEnumerable<byte>? abData)
				=> (abData == null || !abData.Any()) ? "" : Encoding.ASCII.GetString((byte[])(abData!));


			/// <summary>Возвращает строку из байт, включая нечитаемые символы (используется Text.Encoding.Unicode.GetString)</summary>
			/// <param name="abData">Массив байт</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringUnicodeFast(this IEnumerable<byte>? abData)
				=> (abData == null || !abData.Any()) ? "" : Encoding.Unicode.GetString((byte[])(abData!));


			/// <summary>Возвращает строку байт вида 00-00-01-01-05-AA</summary>
			/// <param name="bReverseRTL">Развернуть в программисткий вид (младший байт будет справа)</param>
			/// <param name="sByteSeparator">Рзделитель байтов. Если не указан, используется системный (по-умолчанию) - обычно это минус</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringHex(this IEnumerable<byte>? abData, bool bReverseRTL = false, string? sByteSeparator = null)
			{
				if (null == abData) return "";
				if (bReverseRTL) abData = abData.Reverse();

				string sResult = BitConverter.ToString((byte[])abData);

				if (null != sByteSeparator)
				{
					// Использовать особый разделитель
					char sDefaultSeparator = constants.SystemDefaultHexByteSeparator;
					if (sByteSeparator != Convert.ToString(sDefaultSeparator))
					{
						sResult = sResult.e_ReplaceAll2(Convert.ToString(sDefaultSeparator), sByteSeparator);
					}
				}

				return sResult;
			}



			#region IP Address

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringHex(this System.Net.NetworkInformation.PhysicalAddress MAC) => MAC.GetAddressBytes().e_ToStringHex(false);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringHex(this System.Net.IPAddress IP) => IP.GetAddressBytes().e_ToStringHex(false);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringHex(this System.Net.IPEndPoint IPEP) => $"{IPEP.Address.e_ToStringHex()}-{BitConverter.GetBytes((ushort)IPEP.Port).e_ToStringHex()}";

			#endregion

			/// <summary>Removes only Space char (0x32)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_RemoveSpacesFast(this string source)
				=> source.Replace(" ", string.Empty);

			/// <summary>Removes all Unicode character which is categorized as white space.</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_RemoveSpacesEx(this string source)
				=> string.Concat(source.Where(c => !char.IsWhiteSpace(c)));



			/// <summary>Заменяет все множественные пробелы на один пробел</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_RemoveDoubleSpaces(this string SourceText) => SourceText.e_ReplaceAll2("  ", " ");


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReplaceAll2(this string source, string WhatToFind, string sReplaceWith, bool OnlyFirst = false)
			{
				if (WhatToFind.e_IsNullOrEmpty()) throw new ArgumentNullException(nameof(WhatToFind));
				if (sReplaceWith.Contains(WhatToFind) && !OnlyFirst)
					throw new ArgumentException($"{nameof(sReplaceWith)} = '{sReplaceWith}', and contains WhatToFind = '{WhatToFind}'");

				while (source.Contains(WhatToFind))
				{
					source = source.Replace(WhatToFind, sReplaceWith);
					if (OnlyFirst) break;
				}
				return source;
			}

			/// <summary>Заменяем все вхождения на заданную строку</summary>
			/// <param name="source">Исходный текст</param>
			/// <param name="WhatToFind">Что нужно найти и заменить</param>
			/// <param name="sReplaceWith">На что заменять</param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReplaceAll2(this string source, IEnumerable<string> WhatToFind, string sReplaceWith, bool OnlyOneFind = false)
			{
				if (null == WhatToFind || !WhatToFind.Any()) throw new ArgumentNullException(nameof(WhatToFind));

				foreach (var sFind in WhatToFind) source = source.e_ReplaceAll2(sFind, sReplaceWith, OnlyOneFind);
				return source;
			}


			/// <param name="FindReplacePairs">(FindWhat As String, ReplaceWith As String)</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReplaceAll2(this string SourceText, IEnumerable<(string WhatToFind, string ReplaceWith)> FindReplacePairs, bool OnlyOneFind = false)
			{
				if (null == FindReplacePairs || !FindReplacePairs.Any()) throw new ArgumentNullException(nameof(FindReplacePairs));

				foreach (var frp in FindReplacePairs)
					SourceText = SourceText.e_ReplaceAll2(frp.WhatToFind, frp.ReplaceWith, OnlyOneFind);
				return SourceText;
			}






			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ЗаменитьЗапятыеНаТочки(this string Num)
			{
				return Num.e_ReplaceAll2(",", ".");
			}







			/// <summary>Создаёт строку нулевых символов заданной длинны</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_CreateNullCharSrting(this int iNullCharCount) => new(constants.CC_NULL, iNullCharCount);





			/// <summary>Убирает с конца строку, если она есть</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? e_SubstringFrom(this string SourceText, int iPos)
			{
				if (SourceText.e_IsNullOrWhiteSpace()) return null;
				if (iPos <= 0) return SourceText;
				if (iPos >= SourceText.Length) return null;
				return SourceText.Substring(0, iPos);
			}

			/// <summary>Берёт левую часть строки</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? e_Left(this string SourceText, int iCharCount)
			{
				if (SourceText.e_IsNullOrWhiteSpace()) return null;
				if (iCharCount <= 0) return null;
				if (iCharCount >= SourceText.Length) return SourceText;
				return SourceText[..iCharCount];
			}

			/// <summary>
			/// The example displays the following output: 
			///    Current Culture: en-US 
			///       case = Case (CurrentCulture): False 
			///       case = Case (CurrentCultureIgnoreCase): True 
			///       case = Case (InvariantCulture): False 
			///       case = Case (InvariantCultureIgnoreCase): True 
			///       case = Case (Ordinal): False 
			///       case = Case (OrdinalIgnoreCase): True 
			///     
			///       encyclopædia = encyclopaedia (CurrentCulture): True 
			///       encyclopædia = encyclopaedia (CurrentCultureIgnoreCase): True 
			///       encyclopædia = encyclopaedia (InvariantCulture): True 
			///       encyclopædia = encyclopaedia (InvariantCultureIgnoreCase): True 
			///       encyclopædia = encyclopaedia (Ordinal): False 
			///       encyclopædia = encyclopaedia (OrdinalIgnoreCase): False 
			///     
			///       encyclopædia = encyclopedia (CurrentCulture): False 
			///       encyclopædia = encyclopedia (CurrentCultureIgnoreCase): False 
			///       encyclopædia = encyclopedia (InvariantCulture): False 
			///       encyclopædia = encyclopedia (InvariantCultureIgnoreCase): False 
			///       encyclopædia = encyclopedia (Ordinal): False 
			///       encyclopædia = encyclopedia (OrdinalIgnoreCase): False 
			///     
			///       Archæology = ARCHÆOLOGY (CurrentCulture): False 
			///       Archæology = ARCHÆOLOGY (CurrentCultureIgnoreCase): True 
			///       Archæology = ARCHÆOLOGY (InvariantCulture): False 
			///       Archæology = ARCHÆOLOGY (InvariantCultureIgnoreCase): True 
			///       Archæology = ARCHÆOLOGY (Ordinal): False 
			///       Archæology = ARCHÆOLOGY (OrdinalIgnoreCase): True 
			///     
			///     
			///    Current Culture: se-SE 
			///       case = Case (CurrentCulture): False 
			///       case = Case (CurrentCultureIgnoreCase): True 
			///       case = Case (InvariantCulture): False 
			///       case = Case (InvariantCultureIgnoreCase): True 
			///       case = Case (Ordinal): False 
			///       case = Case (OrdinalIgnoreCase): True 
			///     
			///       encyclopædia = encyclopaedia (CurrentCulture): False 
			///       encyclopædia = encyclopaedia (CurrentCultureIgnoreCase): False 
			///       encyclopædia = encyclopaedia (InvariantCulture): True 
			///       encyclopædia = encyclopaedia (InvariantCultureIgnoreCase): True 
			///       encyclopædia = encyclopaedia (Ordinal): False 
			///       encyclopædia = encyclopaedia (OrdinalIgnoreCase): False 
			///     
			///       encyclopædia = encyclopedia (CurrentCulture): False 
			///       encyclopædia = encyclopedia (CurrentCultureIgnoreCase): False 
			///       encyclopædia = encyclopedia (InvariantCulture): False 
			///       encyclopædia = encyclopedia (InvariantCultureIgnoreCase): False 
			///       encyclopædia = encyclopedia (Ordinal): False 
			///       encyclopædia = encyclopedia (OrdinalIgnoreCase): False 
			///     
			///       Archæology = ARCHÆOLOGY (CurrentCulture): False 
			///       Archæology = ARCHÆOLOGY (CurrentCultureIgnoreCase): True 
			///       Archæology = ARCHÆOLOGY (InvariantCulture): False 
			///       Archæology = ARCHÆOLOGY (InvariantCultureIgnoreCase): True 
			///       Archæology = ARCHÆOLOGY (Ordinal): False 
			///       Archæology = ARCHÆOLOGY (OrdinalIgnoreCase): True
			/// </summary>
			/// <returns></returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_Contains(this string SourceText, string FindWhat, bool IgnoreCase)
			{
				if (null == SourceText || null == FindWhat) return false;
				return (SourceText.IndexOf(FindWhat,
					(IgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
					> 0);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_Contains(this string SourceText, IEnumerable<string> FindWhat, bool IgnoreCase)
			{
				foreach (var S in FindWhat) { if (SourceText.e_Contains(S, IgnoreCase)) return true; }
				return false;
			}

			#region Wrap
			// Friend Const CS_ESCAPE_LF As String = "\n"


			/// <summary>Функция-пустышка - использовалась только в VB</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Obsolete("Функция-пустышка - использовалась только в VB", true)]
			internal static string e_WrapCPP(this string SourceText) => SourceText;


			/// <summary>Заменяет символ "|" на VBCRLF</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Obsolete("Функция-пустышка - использовалась только в VB", true)]
			internal static string e_WrapVB(this string SourceText)
			{
				return SourceText.Replace(Convert.ToString('|'), constants.vbCrLf);
			}

			///// <summary>Вызывает WrapCPP("\n"), затем WrapVB("|") на VBCRLF</summary>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static string e_Wrap(this string SourceText) => SourceText.e_WrapVB();

			#endregion

			/// <summary>Добавляет строку к тексту. Если исходный текст пустой, то разделитель к исходному тексту не добавляется</summary>
			/// <param name="TextToAppend">Строка для добавления</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_AppendText(this string SourceText, string TextToAppend, string sSeparator = constants.vbCrLf)
			{
				if (SourceText.e_IsNullOrWhiteSpace()) SourceText = "";
				if (SourceText.e_IsNOTNullOrWhiteSpace()) SourceText += sSeparator;
				return (SourceText + TextToAppend);
			}
			/// <summary>Добавляет строку к тексту. Если исходный текст пустой, то разделитель к исходному тексту не добавляется</summary>
			/// <param name="TextToAppend">Строка для добавления</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static StringBuilder e_toStringBuilder(this string SourceText)
			{
				return new StringBuilder(SourceText);
			}

			/// <summary>Вырезает кусок строки</summary>
			/// <returns>Возвращает строку без вырезанного текста</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReplaceSubstring(this string SourceText, int ReplaceStart, int ReplaceLenght, string ReplaceWith)
			{
				string sBefore = SourceText.Substring(0, ReplaceStart);
				string sAfter = SourceText.Substring(ReplaceStart + ReplaceLenght);
				SourceText = sBefore + ReplaceWith + sAfter;
				return SourceText;
			}


			/// <summary>Вырезает кусок строки. Возвращает строку без вырезанного текста</summary>
			/// <returns>Возвращает строку без вырезанного текста</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Cut(this string SourceText, int CutStart, int CutLenght)
			{
				string S = SourceText.e_ReplaceSubstring(CutStart, CutLenght, "");
				return S;
			}

			/// <summary>Заменяет в тексте строки &lt;0x0A&gt; на соответствующий символ</summary>
			/// <param name="SourceText">Текст например: aaa&lt;0x0A&gt;bbb&lt;0x0D&gt;</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReplaceSpecialHexChars(this string SourceText)
			{
				const string CS_HEX_PREFIX = "<0x";
				const string REGEXP_HEX_CHARS = CS_HEX_PREFIX + "[0-9a-fA-F]{2}>";
				var E = new System.Text.RegularExpressions.Regex(REGEXP_HEX_CHARS);
				var aM = E.Matches(SourceText).Cast<System.Text.RegularExpressions.Match>().ToArray().Reverse();
				foreach (var M in aM)
				{
					string sHex = M.Value;
					sHex = sHex.Substring(CS_HEX_PREFIX.Length, M.Length - CS_HEX_PREFIX.Length - 1);
					byte B = byte.Parse(sHex, NumberStyles.HexNumber);
					SourceText = SourceText.e_ReplaceSubstring(M.Index, M.Length, B.e_ToChar().ToString());
				}

				return SourceText;
			}



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static char[] e_ToChars(this IEnumerable<byte> abData)
				=> abData.Select<byte, char>((B) => B.e_ToChar()).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToStringLine(this IEnumerable<char> A)
				=> new(A.ToArray());


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string[] e_ToStringArray(this IEnumerable<char> A)
				=> A.Select<char, string>((C) => C.ToString()).ToArray();


			/// <summary>Делит текст на строки, использует StringReader.ReadLine()</summary>
			/// <param name="source">Исходный текст для разделения</param>
			/// <returns>Массив строк исходного текста</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<string> e_SplitToLines(this string? source, bool skipEmptyLines = false, bool trimEachLine = false)
			{
				if (source.e_IsNullOrEmpty()) yield break;

				using StringReader sr = new(source!);
				string? sLine = sr.ReadLine();
				while (null != sLine)
				{
					bool bAdd = true;

					if (trimEachLine) sLine = sLine.Trim();
					if (skipEmptyLines) bAdd = sLine.e_IsNOTNullOrWhiteSpace();
					if (bAdd) yield return sLine;
					sLine = sr.ReadLine();
				}
			}


			/// <summary>Создаёт объект System.Guid из строки</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Guid e_ToGUID(this string GUIDString) => Guid.Parse(GUIDString);


			/// <summary>Убирает с конца строку, если она есть</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_RemoveAtEnd(this string source, string SuffixToRemove)
			{
				if (SuffixToRemove.e_IsNullOrEmpty()) return source;
				while (source.EndsWith(SuffixToRemove) && source.Length >= SuffixToRemove.Length)
				{
					int iTake = source.Length - SuffixToRemove.Length;
					if (iTake == 0) return "";
					source = source.Substring(0, iTake);
				}
				return source;
			}

			/// <summary>Убирает с начала строку, если она есть</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_RemoveAtStart(this string source, string prefixToRemove)
			{
				if (prefixToRemove.e_IsNullOrEmpty()) return source;
				while (source.StartsWith(prefixToRemove))
				{
					if (source.Length <= prefixToRemove.Length) return "";
					source = source.Substring(prefixToRemove.Length);
				}
				return source;
			}


			/// <summary>Возвращает окончание строки после указанного фрагмента</summary>
			/// <param name="SourceText">Исходная строка</param>
			/// <param name="StartWithString">Вернётся отсток, после конца этой строки</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_SubstringAfter(this string SourceText, string StartWithString)
			{
				if (StartWithString.e_IsNullOrWhiteSpace()) return SourceText;
				int iFind = SourceText.IndexOf(StartWithString);
				if (iFind < 0) return SourceText;
				string S = SourceText.Substring(iFind + StartWithString.Length);
				return S;
			}

			/// <summary>Возвращает кусок строки между Prefix и Suffix</summary>
			/// <param name="SourceText">Исходная строка от которой выделяется остаток</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? e_SubstringBetween(this string SourceText, string Prefix, string Suffix)
			{
				if (null == SourceText) return null;
				var S = SourceText.e_SubstringAfter(Prefix);
				if (S.e_IsNullOrWhiteSpace() || Suffix.e_IsNullOrWhiteSpace()) return null;
				int iFind = S.IndexOf(Suffix);
				if (iFind <= 0) return null;
				S = S.Substring(0, iFind);
				return S;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static StringCollection e_ToSpecializedStringCollection(this IEnumerable<string> SourceStrings)
			{
				StringCollection C = new();
				C.AddRange(SourceStrings.ToArray());
				return C;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static List<string> e_ToList(this StringCollection C) => C.Cast<string>().ToList();


			#region MiltiStringZ

			/// <summary>Создаёт одну строку (REG_MULTI_SZ) из элементов массива (где каждая строка отделена [0] а в конце [00])</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToAPIMultiStringZ(this IEnumerable<string> SourceStrings)
			{
				var sResult = SourceStrings!.e_Join('\0'.ToString());
				sResult += '\0'.ToString();
				return sResult;
			}

			/// <summary>Возвращает строки из REG_MULTI_SZ (где каждая строка отделена [0] а в конце [00])</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Obsolete("!!!  e_ReadMiltiStringZUni НАДО ПРОВЕРИТЬ РАБОТОСПОСОБНОСТЬ!!!")]
			internal static string[] e_ReadMiltiStringZUni(this IntPtr Ptr)
			{
				const int UNICODE_CHAR_SIZE = 2;
				var aData = new List<string>();
				do
				{
					// Если Первый байт нулевой, текста нету
					if (Marshal.ReadByte(Ptr) == 0) break;
					var sLine = Marshal.PtrToStringUni(Ptr);
					if (string.IsNullOrEmpty(sLine) || sLine.Length < 1)
					{
						// Вроде нет больше строк
						break;
					}
					else
					{
						aData.Add(sLine);
						int iOffset = (sLine.Length + 1) * UNICODE_CHAR_SIZE;
						Ptr += iOffset;
					}
				}
				while (true);
				var sVaues = aData.ToArray();
				return sVaues;
			}
			#endregion



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int[] e_AllIndexesOfAsArray(this string? str, string? searchString)
				=> str.e_AllIndexesOf(searchString).ToArray();

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<int> e_AllIndexesOf(this string? str, string? searchString)
			{
				if (null == str || null == searchString) yield break;
				int minIndex = str.IndexOf(searchString);
				while (minIndex != -1)
				{
					yield return minIndex;
					minIndex = str.IndexOf(searchString, minIndex + searchString.Length);
				}
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int[] e_AllIndexesOfAsArray(this string? str, char? c)
				=> str.e_AllIndexesOf(c).ToArray();

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<int> e_AllIndexesOf(this string? str, char? c)
			{
				var result2 = str?
					.Select((b, i) => b.Equals(c) ? i : -1)?
					.Where(i => i != -1);

				if (null == str || null == c) yield break;
				int minIndex = str.IndexOf(c.Value);
				while (minIndex != -1)
				{
					yield return minIndex;
					minIndex = str.IndexOf(c.Value, minIndex + 1);
				}
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_CountOfCharIsAtLeast(this string str, char c, int countAtLeast)
			{
				char[] cc = str.ToCharArray();

				int iCharCount = 0;
				int iCharOld = -1;

				while (iCharCount < countAtLeast)
				{
					int iCharNew = Array.IndexOf(cc, c, iCharOld + 1);
					if (iCharNew == -1) return false;//Not found Next
					iCharOld = iCharNew;
				}
				return true;//Found all
			}



			/// <summary>Добавляет перед и после строки указанный символ</summary>
			/// <param name="enclosestring">Строка добавляемая перед и после строки</param>
			/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Enclose(
				this string source,
				string enclosestring = constants.CS_QUOTE,
				bool notEncloseIfExist = true,
				bool atStart = true,
				bool atEnd = true)
			{
				if (!notEncloseIfExist | !source.StartsWith(enclosestring) && atStart) source = enclosestring + source;
				if (!notEncloseIfExist | !source.EndsWith(enclosestring) && atEnd) source += enclosestring;
				return source;
			}
			/// <summary>Добавляет перед и после строки указанный символ</summary>
			/// <param name="encloseChars">Строка добавляемая перед и после строки</param>
			/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_EncloseC(
				this string source,
				char enclosechar = constants.CC_QUOTE,
				bool notEncloseIfExist = true,
				bool atStart = true,
				bool atEnd = true)
				=> source.e_Enclose(enclosechar.ToString(), notEncloseIfExist, atStart, atEnd);


			/// <summary>Удаляет окружающие строку кавычки или иной окружающий текст</summary>
			/// <param name="SourceText"></param>
			/// <param name="encloseChars"></param>
			/// <param name="onlyOnePass">Только один проход</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Exclose(
				this string SourceText,
				char encloseChars = constants.CC_QUOTE,
				bool onlyOnePass = false)
			{
				while (SourceText.e_IsNOTNullOrWhiteSpace() && SourceText.StartsWith(Convert.ToString(encloseChars)))
				{
					SourceText = SourceText.e_TrimStartOne(encloseChars);
					if (onlyOnePass) break;
				}

				while (SourceText.e_IsNOTNullOrWhiteSpace() && SourceText.EndsWith(Convert.ToString(encloseChars)))
				{
					SourceText = SourceText.e_TrimEndOne(encloseChars);
					if (onlyOnePass) break;
				}
				return SourceText;
			}

			/// <summary>Возрвщвем количество одинаковых символов с начла строки, пока они совпадают с заданными</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_TakeWhile_Count(this string SourceText, char cFirstCharToSelect)
			{
				if (SourceText.e_IsNullOrWhiteSpace() || !SourceText.StartsWith(cFirstCharToSelect.ToString())) return 0;
				int iCount = 0;
				foreach (char C in SourceText)
				{
					if (C != cFirstCharToSelect)
						break;
					iCount += 1;
				}

				return iCount;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_TrimStartOne(this string SourceText, char TrimChar)
			{
				if (SourceText.e_IsNOTNullOrWhiteSpace() && SourceText.StartsWith(Convert.ToString(TrimChar)))
				{
					if (SourceText.Length > 1)
					{
						SourceText = SourceText.Substring(1);
					}
					else
					{
						SourceText = "";
					}
				}

				return SourceText;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_TrimEndOne(this string SourceText, char TrimChar)
			{
				if (SourceText.e_IsNOTNullOrWhiteSpace() && SourceText.EndsWith(Convert.ToString(TrimChar)))
				{
					if (SourceText.Length > 1)
					{
						SourceText = SourceText.Substring(0, SourceText.Length - 1);
					}
					else
					{
						SourceText = "";
					}
				}

				return SourceText;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_TrimEndOne(this string SourceText, string TrimSuffix)
			{
				if (SourceText.e_IsNOTNullOrWhiteSpace() && SourceText.EndsWith(TrimSuffix))
				{
					if (SourceText.Length > TrimSuffix.Length)
					{
						return SourceText.Substring(0, SourceText.Length - TrimSuffix.Length);
					}
					else
					{
						SourceText = "";
					}
				}

				return SourceText;
			}

			/// <summary>Добавляет в конце строки указанный символ</summary>
			/// <param name="SourceText">Исходная строка</param>
			/// <param name="sAppendix">Строка добавляемая в конце исходной</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_FinishWith(this string SourceText, string sAppendix = "")
			{
				if (!SourceText.EndsWith(sAppendix))
				{
					SourceText += sAppendix;
				}

				return SourceText;
			}

			internal enum CHANGE_CASE_MODES : int
			{
				TO_LOWER,
				TO_UPPER,
				TO_TITLE
			}

			/// <summary> преобразует первый символ каждого слова в верхний регистр, а остальные символы — в нижний. 
			/// Однако слова, состоящие только из прописных букв, считаются сокращениями и не преобразуются.
			/// Метод TextInfo.ToTitleCase учитывает регистр, то есть он использует соглашения об использовании регистров, действующие для определенного языка и региональных параметров. 
			/// Чтобы вызвать этот метод, сначала нужно получить объект TextInfo, представляющий соглашения об использовании регистров, из свойства CultureInfo.TextInfo конкретного языка и региональных параметров.
			/// 
			/// В примере ниже каждая строка из массива передается в метод TextInfo.ToTitleCase. 
			/// Среди строк есть как строки заголовков, так и сокращения. Строки преобразуются в последовательности слов, начинающихся с заглавных букв, согласно соглашениям об использовании регистров для языка и региональных параметров Английский (США). </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ToTitleCase(this string Source)
			{
				var CI = CultureInfo.CurrentCulture;
				string B = CI.TextInfo.ToTitleCase(Source);
				return B;
			}


			/// <summary>Добавляет пробелы перед заглавными буквами</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_SplitCasedWord(this string Source)
			{
				if (Source.e_IsNullOrWhiteSpace() || Source.Length < 2) return Source;

				var sbResult = new StringBuilder(Source.Length * 2);
				var aWordChars = Source.ToCharArray();
				bool bFirst = true;
				foreach (char C in aWordChars)
				{
					if (!bFirst && char.IsUpper(C)) sbResult.Append(' ');
					sbResult.Append(C);
					bFirst = false;
				}
				return sbResult.ToString();
			}

			/// <summary>Проверяет строку на (Is Nothing) и (String.IsNullOrEmpty) и (SourceString.Length>0) и возвращает либо исходную строку, либо пустую</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_CheckNullChar(this char SourceChar, string DefaultValue = "")
				=> (SourceChar == constants.vbNullChar) ? DefaultValue : SourceChar.ToString();


			/// <summary>Проверяет строку на (Is Nothing) и (String.IsNullOrEmpty) и (SourceString.Length>0) и возвращает либо исходную строку, либо пустую</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_CheckNullOrWhiteSpace(this string SourceText, string DefaultValue = "")
				=> SourceText.e_IsNullOrWhiteSpace() ? DefaultValue : SourceText;





			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_TakeReadableChars(this IEnumerable<byte> abData, byte NotReadableMinValue = 31)
				=> abData.Where((B) => (B > NotReadableMinValue)).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static char[] e_GetDigitChars(this IEnumerable<char> aPhoneChars)
				=> aPhoneChars.Where((C) => char.IsDigit(C)).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? e_GetDigitChars(this string S)
			{
				if (S.e_IsNullOrWhiteSpace()) return null;
				var AB = S.ToArray().e_GetDigitChars();
				return (!AB.Any()) ? null : new string(AB);
			}







			#region API Related / Win / DOS / NString

			/// <summary>Взвращает строку слева до символа с кодом 0 (ноль)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_NString(this string SourceText)
			{
				string sText = SourceText.e_CheckNullOrWhiteSpace();
				int iZeroPos = sText.IndexOf(constants.vbNullChar);
				switch (iZeroPos)
				{
					case < 0: break;                    // Нет нулевого символа в строке
					case 0: return "";                            // Нулевой символ это первый символ
					case > 0: sText = sText.Substring(0, iZeroPos); break;// Есть нулевой символ в строке
				}
				return sText;
			}

			// ''' <summary>Убирает все символы с кодом 0 справа до первого ненулевого символа</summary>
			// <System.Runtime.CompilerServices.Extension()> _
			// Friend Function TrimmNullCharsRight(ByVal sText As String) As String
			// If (sText Is Nothing) Then Return ""
			// Dim sNewText As String = sText
			// While sNewText.EndsWith(Chr(0))
			// sNewText = sNewText.Substring(0, sNewText.Length - 1)
			// End While
			// Return sNewText
			// End Function

			#region Encoding Win-Dos


			// <DllImport(  UOM.Win32.WINDLL_USER, _
			// SetLastError:=True, _
			// CharSet:=CharSet.Auto, _
			// ExactSpelling:=False, _
			// CallingConvention:=CallingConvention.Winapi)> _
			// Friend Overloads Shared Function CharToOemBuff( _
			// <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As String, _
			// <MarshalAs(UnmanagedType.LPStr)> ByVal lpszDst As String, _
			// ByVal cchDstLength As Integer) As Integer
			// '
			// End Function

			// <DllImport(UOM.Win32.WINDLL_USER, SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=False, CallingConvention:=CallingConvention.Winapi)>
			// Private Function CharToOemBuff(<[In](), MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As String,
			// ByVal lpszDst() As Byte,
			// ByVal cchDstLength As Integer) As Integer
			// End Function

			// ''' <summary>Перевод строки в DOS кодировку</summary>
			// ''' <param name="Строка_в_WIN_кодировке"></param>
			// Friend Function  e_CharToOem(ByVal Строка_в_WIN_кодировке As String) As Byte()
			// Dim abDOS(Строка_в_WIN_кодировке.Length - 1) As Byte
			// Call CharToOemBuff(Строка_в_WIN_кодировке, abDOS, abDOS.Length)
			// Dim WEX As New System.ComponentModel.Win32Exception
			// If (WEX.NativeErrorCode <> 0) Then Throw WEX
			// 'sText =  UOM.Convert.CharToOemBuff(sText)
			// 'Dim sDos As String = Строка_в_WIN_кодировке
			// 'Call CharToOemBuff(Строка_в_WIN_кодировке, sDos, Len(Строка_в_WIN_кодировке))
			// Return abDOS
			// End Function

			// 'Friend Overloads Shared Function  e_CharToOemBuff(ByVal Строка_в_WIN_кодировке As String) As String
			// '    Dim sDos As String = Строка_в_WIN_кодировке
			// '    Call CharToOemBuff(Строка_в_WIN_кодировке, sDos, Len(Строка_в_WIN_кодировке))
			// '    Return sDos
			// 'End Function


			// <DllImport(UOM.Win32.WINDLL_USER, SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=False, CallingConvention:=CallingConvention.Winapi)>
			// Private Function OemToCharBuff(<MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As String,
			// <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszDst As String,
			// ByVal cchDstLength As Integer) As Integer
			// End Function
			// Friend Function  e_OemToChar(ByVal Строка_в_DOS_кодировке As String) As String
			// Dim sWin As String = Строка_в_DOS_кодировке
			// Call OemToCharBuff(Строка_в_DOS_кодировке, sWin, Строка_в_DOS_кодировке.Length)
			// Return sWin
			// End Function


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ConvertEncoding(this string TextToConvert, string FromEncoding, string ToEncoding)
			{
				var encFrom = Encoding.GetEncoding(FromEncoding);
				var encTo = Encoding.GetEncoding(ToEncoding);
				return TextToConvert.e_ConvertEncoding(encFrom, encTo);
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ConvertEncoding(this string TextToConvert, Encoding encFrom, Encoding encTo)
			{

				// #$OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")
				// #[Console]::OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")	
				// #chcp 1251 - установить кодовую страницу, соответствующую Windows-кодировке.		
				// #1251 – Windows (кириллица);
				// #866 – DOC-кодировка;
				// #65001 – UTF-8;

				var abData = encTo.GetBytes(TextToConvert);
				abData = Encoding.Convert(encFrom, encTo, abData);
				return encTo.GetString(abData);
			}

			/// <summary>Преобразование текста из cp866 в windows-1251</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ConvertEncoding_Dos_Windows_1251(this string TextToConvert)
				=> TextToConvert.e_ConvertEncoding(constants.LEncoding_cp866.Value, constants.LEncoding_Windows1251.Value);
			#endregion

			#endregion



			/// <summary>Create string like '\\x.x.x.x\' or \\server\</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_CreateWinSharePrefix(this string host) => $@"\\{host}\";


		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_Arrays
		{

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_ForEach<T>(this IEnumerable<T>? objList, Action<T>? action)
			{
				foreach (T item in objList?.e_OrEmptyIfNull()!)
					action?.Invoke(item);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] e_Sort<T>(this IEnumerable<T> source) where T : System.IComparable<T>
			{
				_ = source ?? throw new ArgumentNullException(nameof(source));
				return (from N in source orderby N select N).ToArray();
			}

			/// <summary>Return string that contains in source text</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<string> e_Contains(this string? source, IEnumerable<string>? aWhatToFind)
				=> (null == source || null == aWhatToFind) ? Array.Empty<string>() : aWhatToFind!.Where(S => source!.Contains(S));

			/// <summary>Return true if any string contains in source text</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_ContainsAny(this string? source, IEnumerable<string>? aWhatToFind) => source.e_Contains(aWhatToFind).Any();

			/// <summary>Creates 1 element array with specifed item</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] e_ToArrayOf<T>(this T? source) => (null == source) ? System.Array.Empty<T>() : new T[] { source };

			/*

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T? e_FirstIfContains<T>(this IEnumerable<T> A, Func<T, bool> ContainCheck)
			{
				if (!A.Any()) return default;

				foreach (var R in A)
				{
					if (ContainCheck.Invoke(R)) return R;
				}
				return default;
			}
			 */

			/// <summary>Return source or Empty<source> if source is null</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<T> e_OrEmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();

			/// <summary>Checks source to null or not ANY()</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsEmptyOrNull<T>(this IEnumerable<T>? items) => ((null == items) || !items.Any());

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsNotEmptyOrNull<T>(this IEnumerable<T>? items) => !items.e_IsEmptyOrNull();



			/// <summary>Возвращает массив строк, где каждая строка = 'ключ = значение'</summary>
			/// <param name="dic"></param>
			/// <param name="skipNullValues"></param>
			/// <param name="nullValuesDisplayName"></param>
			/// <returns></returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string[] e_ToArrayOfString<T>(
				this Dictionary<string, T> dic,
				bool skipNullValues = true,
				string nullValuesDisplayName = "[NULL]")
				=> dic
				.Select<KeyValuePair<string, T>, string?>(kvp
					=> ((null == kvp.Value) && skipNullValues)
					? null
					: $"{kvp.Key} = {kvp.Value?.ToString() ?? nullValuesDisplayName}")
				.Where(s => (null != s))
				.ToArray()!;


			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string[]? e_ToArrayOfString(this System.Collections.Specialized.StringCollection? sc)
				=> sc?.Cast<string>()?.ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] e_CreateArray<T>(this T fillWith, int iCount)
			{
				T[] arr = new T[iCount];

#if NET5_0_OR_GREATER
				System.Array.Fill(arr, fillWith);
#else
				for (int n = 1; n < iCount; n++) arr[n] = fillWith;
#endif
				return arr;
			}

			// ''' <summary>Очень медленно!</summary>
			// <DebuggerNonUserCode, DebuggerStepThrough>
			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function  e_CreateArray2(Of T)(tFillWithItems As T, ItemsCount As Integer) As T()
			// Dim aResult = Enumerable.Range(1, ItemsCount).Select(Function(X) tFillWithItems).ToArray
			// Return aResult
			// End Function

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] e_AddRange<T>(this T[] arr, T[] arrAppend)
			{
				var lData = arr.ToList();
				lData.AddRange(arrAppend);
				return lData.ToArray();
			}

			/// <summary>Null Safe ANY implementation</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_Any<T>(this IEnumerable<T>? a) => null != a && a.Any();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_ContainsAnyOf<T>(this IEnumerable<T> Arr1, IEnumerable<T> Arr2)
			{
				foreach (var Element2 in Arr2)
					if (Arr1.Contains(Element2)) return true;

				return false;
			}


			/// <summary>Возвращает элементы массива начиная с заданного</summary>
			/// <param name="iStartTakeIndex">Zero-based start char index</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_TakeFrom(this string S, int iStartTakeIndex) => S.Substring(iStartTakeIndex);


			/// <summary>Возвращает элементы массива начиная с заданного</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] e_TakeFrom<T>(this T[] A, int iStartTakeIndex)
			{
				if (A.Length <= iStartTakeIndex) return Array.Empty<T>();
				var L = A.Length - iStartTakeIndex;
				var aResult = new T[L];
				Array.Copy(A, iStartTakeIndex, aResult, 0, L);
				return aResult;
			}


			///// <summary>Возвращает первые элементы набора в виде массива</summary>
			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static T[] e_TakeFirst<T>(this IEnumerable<T> A, int iCount)
			//{
			//    if (A.Count() <= iCount) return A.ToArray();
			//    var B = A.Take(iCount).ToArray();
			//    return B;
			//}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] e_RemoveFirst<T>(this T[] L, int iCount = 1)
			{
				var LL = L.ToList();
				LL.e_RemoveFirst(iCount);
				L = LL.ToArray();
				return L;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_RemoveFirst<T>(this List<T> L, int iCount = 1)
			{
				for (int n = 1; n <= iCount; n++) L.RemoveAt(0);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_RemoveLast<T>(this List<T> L) => L.RemoveAt(L.Count - 1);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_RemoveRange<T>(this List<T> L, IEnumerable<T> ItemsToRemove)
				=> ItemsToRemove.e_ForEach(rItem => L.Remove(rItem));


			/// <summary>Объединяет двумерный массив в одномерный. !!!БЕЗ СОРТИРОВКИ!!!</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] e_Merge2Dto1D<T>(this IEnumerable<IEnumerable<T>> Array2D)
			{
				var lResult = new List<T>();
				Array2D.e_ForEach(arrSecondLevel => lResult.AddRange(arrSecondLevel.ToArray()));
				return lResult.ToArray();
			}


			/// <summary>Считываем первый элемент списка, и удаляем его из списка (как в стеке)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T? e_PeekFirstOrDefault<T>(this List<T> L)
			{
				var TmpVal = L.FirstOrDefault();
				if (L.Any()) L.RemoveAt(0);
				return TmpVal;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_SelectSwitch<T>(this int ReturnIndex, params T[] ReturnValues) => ReturnValues[ReturnIndex];






			/*

			internal enum NEAREST_SEARCH_MODES : int
			{
				eSmallerOrEqual,
				eSmallerOnly,
				eLargerOrEqual,
				eLargerOnly
			}

			/// <summary>Находит индекс в массиве ближайшего меньшего или большего числа</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int e_GetNearestIndex(this int[] aSource, int NearValue, NEAREST_SEARCH_MODES eMode)
			{
				switch (eMode)
				{
					case NEAREST_SEARCH_MODES.eSmallerOrEqual:
					case NEAREST_SEARCH_MODES.eSmallerOnly:
						{
							var aSmaller = (from I in aSource
											where I <= NearValue
											orderby I
											select I).ToArray();
							if (eMode == NEAREST_SEARCH_MODES.eSmallerOnly)
								aSmaller = aSmaller.Except(new int[] { NearValue }).ToArray(); // исключаем само число

							if (!aSmaller.Any()) return -1; // Нет ни одного элемента меньше заданного

							int iLast = aSmaller.Last();
							int iPos = Array.IndexOf(aSource, iLast);
							return iPos;
						}

					default:
						{
							var aLarger = (from I in aSource
										   where I >= NearValue
										   select I).ToArray();
							if (eMode == NEAREST_SEARCH_MODES.eLargerOnly)
								aLarger = aLarger.Except(new int[] { NearValue }).ToArray(); // исключаем само число
							if (!aLarger.Any())
								return -1; // Нет ни одного элемента больше заданного
							int iFirst = aLarger.First();
							int iPos = Array.IndexOf(aSource, iFirst);
							return iPos;
						}
				}
			}
			*/


			///// <summary>Сдвиг на 1 элемент влево (удаление первого элемента)</summary>
			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static IEnumerable<T> e_Shift<T>(this IEnumerable<T> ArrayToShift)
			//{
			//    _ = ArrayToShift ?? throw new ArgumentNullException(nameof(ArrayToShift));
			//    if (!ArrayToShift.Any()) return Array.Empty<T>();

			//    var L = ArrayToShift.ToList();
			//    L.RemoveAt(0);
			//    return L;
			//}


			#region Compare Arrays






			/// <summary>Поэлементное сравнение массивов</summary>'
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_CompareArrays_ByElements<T>(this T[] arrA, T[] arrB, int iCompareCount = 0) where T : IComparable<T>
			{
				if (arrA.Length != arrB.Length) return false;
				if (!arrA.Any()) return true;

				if (iCompareCount < 1) iCompareCount = arrA.Length;

				for (int n = 0; n < iCompareCount; n++)
					if (arrA[n].CompareTo(arrB[n]) != 0) return false;

				return true;
			}


			/// <summary>uses Enumerable.SequenceEqual() 
			/// Это один из самых простых методов, самый простой для реализации. Кроме того, это самый популярный метод.</summary>'
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_CompareArrays_Linq<T>(this T[] arrA, T[] arrB) where T : IComparable<T>
			{
				if (arrA.Length != arrB.Length) return false;
				if (!arrA.Any()) return true;

				return arrA.SequenceEqual(arrB);
			}


			/// <summary>Использование интерфейса <see cref="System.Collections.IStructuralEquatable"/> - Это новый способ, появился только в NET_4</summary>'
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_CompareArrays_StructuralEquatable<T>(this T[] arrA, T[] arrB) where T : IStructuralEquatable, IComparable<T>
			{
				if (arrA.Length != arrB.Length) return false;
				if (!arrA.Any()) return true;

				return (arrA as IStructuralEquatable).Equals(arrB as IStructuralEquatable, StructuralComparisons.StructuralEqualityComparer);
			}

			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET5_0_OR_GREATER
			/// <summary>
			/// Решение на основе векторов из System.Numerics
			/// Теоретически должно работать с аппаратным ускорением, но надо чтобы размер массива был не менее системного размера вектора)
			/// https://docs.microsoft.com/ru-ru/dotnet/api/system.numerics.vector-1?view=net-6.0
			/// </summary>'
			internal static bool e_CompareArrays_Vector<T>(this T[] arrA, T[] arrB) where T : struct
			{
				if (arrA.Length != arrB.Length) return false;

				int platformVectorSize = System.Numerics.Vector<T>.Count;   //On x64 Windows platformVectorSize = 32 byte
																			//bool hwa = System.Numerics.Vector.IsHardwareAccelerated;

				System.Numerics.Vector<T> va, vb;
				int i = 0;

				// Compare main body by blocks,
				// with block size = hardware accelerated platformVectorSize
				int iMax = (arrA.Length - platformVectorSize);
				for (; i <= iMax; i += platformVectorSize)
				{
					va = new(arrA, i);
					vb = new(arrB, i);
					if (!System.Numerics.Vector.EqualsAll(va, vb)) return false;
				}

				// Compare Tail
				if (arrA.Length < platformVectorSize)
				{
					Array.Resize(ref arrA, platformVectorSize);
					Array.Resize(ref arrB, platformVectorSize);
					i = 0;
				}
				else
				{
					i = arrA.Length - platformVectorSize;
				}
				va = new(arrA, i);
				vb = new(arrB, i);

				return System.Numerics.Vector.EqualsAll(va, vb);
			}
#endif

			/*
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_ShiftL(this ref int[] arr, int steps = 1, int fillEmpty = default)
			{
				Array.Copy(arr, 0 + steps, arr, 0, arr.Length - steps);
				Span<int> sp = new(arr, arr.Length - steps, steps);
				sp.Fill(fillEmpty);


				//var sl = new Memory<T>();				
				//int bytes2copy = yourArray.length - 4;
				//Buffer.BlockCopy(yourArray, 4, yourArray, 0, bytes2copy);
				//yourArray[yourArray.length - 1] = null;

		}

		internal static void e_SwapWith<T>(this ref T x, ref T y)
		{
			T tmp = x; x = y; y = tmp;
		}

			 */
			#endregion


		}


		internal static class Extensions_ListsSync
		{

			#region SYNC Engine

			internal delegate bool IsEqualDelegate<TOld, TNew>(TOld OldItem, TNew NewItem);

			internal delegate void UpdateOldItemDelegate<TOld, TNew>(TOld OldItem, TNew NewItem);

			/// <summary>Синхронизирует 2 списка.
			/// Сравнивает "старый" список существующих элементов, с "новым" списком, 
			/// чтобы определить какие элементы из старого списка отсутствуют в новом, 
			/// и каких новых элементов нет в старом списке.</summary>
			/// <typeparam name="TCurrent">Тип сущестующего списка</typeparam>
			/// <typeparam name="TNew">Тип нового списка</typeparam>
			/// <param name="OldList">Текущий список элементов</param>
			/// <param name="NewList">Список новых элементов</param>
			/// <param name="CompareFunc">Сравниватель новых и старых элементов</param>
			/// <param name="UpdateOldItemCallback">Вызывается, когда в новом списке есть такой-же элемент и его данными можно обновить старый список</param>
			/// <param name="OnItemObsolete">Элемент старого списка отсутствует в новом. Его можно удалить или пометить, как старый</param>
			/// <param name="OnNewItemNeedToAdd">New item need to be added to old list</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_Sync<TCurrent, TNew>(
				this IEnumerable<TCurrent> OldList,
				IEnumerable<TNew> NewList,
				IsEqualDelegate<TCurrent, TNew> CompareFunc,
				UpdateOldItemDelegate<TCurrent, TNew>? UpdateOldItemCallback = null,
				Action<TCurrent>? OnItemObsolete = null,
				Action<TNew>? OnNewItemNeedToAdd = null)
			{
				if (!OldList.Any() && !NewList.Any()) return;

				// В старом списке ищем записи, кторорых нет в новых данных, для пометки как устаревшие и возможного удаления
				foreach (var rOld in OldList)
				{
					// Для каждого существующего элемента проверяем есть ли он в новом списке
					var OldRecordsNeedToBeUpdated = NewList.Where(rNew => CompareFunc.Invoke(rOld, rNew));
					if (OldRecordsNeedToBeUpdated.Any())
					{
						// New list contains any elements as in Old list
						if (UpdateOldItemCallback != null)
							OldRecordsNeedToBeUpdated.e_ForEach(rNewItem => UpdateOldItemCallback?.Invoke(rOld, rNewItem));// Обновляем элемент старого списка новыми значениями
					}
					else // В новом списке нет этого старого элемента
						OnItemObsolete?.Invoke(rOld);// Сообщаем что элемент устарел (РЕШИТЬ ВОПРОС С ВОЗМОЖНОСТЬЮ УДАЛЕНИЯ!!!) Внутри вызова можно удалять элемент, т.к. щас смотрим array-копию старого списка и это безопасно
				}

				// Ищем новые элементы, которых нет в старом списке
				foreach (var rNew in NewList)
				{
					// Для каждого элемента нового списка проверяем есть ли он в старом списке
					var aНовыйЭлеменНайденВСтаромСписке = OldList.Where(rOld => CompareFunc.Invoke(rOld, rNew));
					if (aНовыйЭлеменНайденВСтаромСписке.Any())
					{
						//
					}
					// В старом списке есть такой же элемент (элементы уже обновлены в первом абзаце)
					else // В старом списке нет этого нового элемента
						OnNewItemNeedToAdd?.Invoke(rNew);
				}
			}
			#endregion

		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_Enum
		{


			//public static bool e_Has<T>(this System.Enum type, T value)
			//{
			//    try { return (((int)(object)type & (int)(object)value!) == (int)(object)value); }
			//    catch { return false; }
			//}

			public static bool e_Is<T>(this System.Enum type, T value)
			{
				try { return (int)(object)type == (int)(object)value!; }
				catch { return false; }
			}

			public static T e_Add<T>(this System.Enum type, T value)
			{
				try { return (T)(object)(((int)(object)type | (int)(object)value!)); }
				catch (Exception ex) { throw new ArgumentException($"Could not append value from enumerated type '{typeof(T).Name}'", ex); }
			}

			public static T e_Remove<T>(this System.Enum type, T value)
			{
				try { return (T)(object)(((int)(object)type & ~(int)(object)value!)); }
				catch (Exception ex) { throw new ArgumentException($"Could not remove value from enumerated type '{typeof(T).Name}'", ex); }
			}


















			///<summary>Return value of <see cref="System.ComponentModel.DescriptionAttribute"/></summary>    
			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string? e_GetDescriptionValue(this Enum rEnumValue, string? DefaultValue = null)
			{
				var T = rEnumValue.GetType();
				var bIsDirectDefinedEnumValue = (Enum.IsDefined(T, rEnumValue));
				if (bIsDirectDefinedEnumValue)
				{ //This value id direct defined in ENUM! not bit mask
					var sFieldName = rEnumValue.ToString();
					var aDA = T.GetField(sFieldName)!.GetCustomAttributes(typeof(DescriptionAttribute), true).ToArray();
					if (aDA!.Any()) return (aDA.First() as DescriptionAttribute)!.Description;
					return DefaultValue;

				}
				else
				{ //This is bitmasked enum, build result from flags
					var lDescriptionsList = new List<string>(10);
					var aAllEnumValues = Enum.GetValues(T).Cast<Enum>().ToArray(); //Все значения этого ENUMA
					foreach (var eFieldValue in aAllEnumValues)
					{
						var bHasThisFlags = rEnumValue.HasFlag(eFieldValue);
						if (bHasThisFlags)
						{
							var sFieldName = eFieldValue.ToString();
							var aDA = T.GetField(sFieldName)!.GetCustomAttributes(typeof(DescriptionAttribute), true).ToArray();
							if (aDA!.Any()) sFieldName = (aDA.First() as DescriptionAttribute)!.Description;
							lDescriptionsList.Add(sFieldName);
						}
					}
					var S = lDescriptionsList.e_Join(", ");
					return S;
				}
			}



			/*




		////// <summary>Возвращает значение атрибута <see cref="My.UOM.EnumTools.Description2Attribute"/> </summary>    
		<Obsolete>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>

		Friend Function ExtEnum_GetDescription2Value(ByVal E As[Enum]) As String
		return New My.UOM.EnumTools.EnumTypeConverter(E.typeof).ConvertToString(E)
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetDescription2Value(EnumValue As [Enum],
		 rObject As Object,
		 EnumValueFieldName As String,
		 Optional bTrowErrorIfCommentAttributeNotFound As bool  = true) As String

		var OT = rObject.typeof
		var S = ExtEnum_GetDescription2Value(EnumValue, OT, EnumValueFieldName, bTrowErrorIfCommentAttributeNotFound)
		return S
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetDescription2Value(EnumValue As [Enum],
		 rObjectType As System.Type,
		 EnumValueFieldName As String,
		 Optional bTrowErrorIfCommentAttributeNotFound As bool  = true) As String


		var sEnumValue = EnumValue.ToString

		var VT = EnumValue.typeof
		var bIsDefinedInEnum = [Enum].IsDefined(VT, EnumValue)
		if(Not bIsDefinedInEnum) {
		if(Not bTrowErrorIfCommentAttributeNotFound) { return sEnumValue
		var sErr = "Enum {0} не содержит элемента = {1}!". e_Format(VT.ToString, sEnumValue)
		return sEnumValue
		}

		var OT = rObjectType
		var aProperties = OT.GetMember(EnumValueFieldName).ToArray
		if(Not aProperties.Any) {
		//if (Not bTrowError) { return sEnumValue
		var sErr = "Класс //{0}// не содержит поля или свойства //{1}//!". e_Format(OT.ToString, EnumValueFieldName)
		Throw New ArgumentOutOfRangeException(EnumValueFieldName, sErr)
		ElseIf(aProperties.Count<> 1) {
		//if (Not bTrowError) { return sEnumValue
		var sErr = "Класс //{0}// содержит БОЛЕЕ ОДГОГО поля или свойства //{1}//!". e_Format(OT.ToString, EnumValueFieldName)
		Throw New ArgumentOutOfRangeException(EnumValueFieldName, sErr)
		}

		var rFirstProperty = aProperties.First
		var ECAT = typeof(My.UOM.EnumTools.Description2Attribute)
		var aAttrs = rFirstProperty.GetCustomAttributes(ECAT, false)
		var aAttrs2 = aAttrs.Cast(Of My.UOM.EnumTools.Description2Attribute).ToArray
		if Not aAttrs2.Any { //Не найден  ни один аттрибут
		if(Not bTrowErrorIfCommentAttributeNotFound) { return sEnumValue
		var sErr = "У свойства //{0}.{1}// не задан ни один аттрибут //{2}!". e_Format(OT.ToString, EnumValueFieldName, ECAT.ToString)
		Throw New ArgumentException(sErr)
		}

		For Each rAttr In aAttrs2
		var eAttr As System.Enum = CType(rAttr.Value, [Enum])
		if eAttr.CompareTo(EnumValue) = 0 {
		sEnumValue = rAttr.Description
		return sEnumValue
		}
		Next

		return sEnumValue
		End Function












	# Region "CBO EnumContainer Helpers"
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_ItemsAsEnumContainers(Of T)(CBO As ComboBox) As List(Of My.UOM.EnumTools.EnumContainer(Of T))
		var aWrappers = (From O As Object In CBO.Items
		Let OO = DirectCast(O, My.UOM.EnumTools.EnumContainer(Of T))
		Select OO).ToArray

		return aWrappers.ToList
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_IndexOfEnumContainerForItem(Of T)(CBO As ComboBox, EnumItem As T) As Integer
		var aContainers = CBO.ExtEnum_ItemsAsEnumContainers(Of T)()
		For N = 1 To aContainers.Count
		var iFound = (N - 1)
		if (aContainers(iFound).Value.Equals(EnumItem)) {
		return iFound
		}
		Next
		return -1
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetEnumContainerForItem(Of T)(CBO As ComboBox, Item As T) As My.UOM.EnumTools.EnumContainer(Of T)
		var aWrappers = CBO.ExtEnum_ItemsAsEnumContainers(Of T)()
		var aFound = (From O In aWrappers Where(O.Value.Equals(Item)))

		if(Not aFound.Any) { return Nothing
		var F = aFound.First
		return F
		End Function


	# Region "EnumContainer"


		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_ItemAsEnumcontainer(Of T)(ByVal CBO As ComboBox, ItemIndex As Integer) As My.UOM.EnumTools.EnumContainer(Of T)
		var Obj = CBO.Items(ItemIndex)
		var EC As My.UOM.EnumTools.EnumContainer(Of T) = DirectCast(Obj, My.UOM.EnumTools.EnumContainer(Of T))
		return EC
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SelectedItemAsEnumcontainer(Of T)(ByVal CBO As ComboBox) As My.UOM.EnumTools.EnumContainer(Of T)
		var Obj = CBO.SelectedItem
		var EC = DirectCast(Obj, My.UOM.EnumTools.EnumContainer(Of T))
		return EC
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SelectedItemAsEnumContainerValue(Of T)(ByVal CBO As ComboBox) As T
		var objSel = CBO.ExtEnum_SelectedItemAsEnumcontainer(Of T)()
		var V = objSel.Value
		return V
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SelectedItemAsEnumContainerValue(Of T)(CBO As ToolStripComboBox) As T
		return CBO.ComboBox.ExtEnum_SelectedItemAsEnumContainerValue(Of T)
		End Function


	# End Region


		//////<summary>Заполняет ComboBox лбъектами типа <see cref="My.UOM.EnumTools.EnumContainer"/>, и выбирает текущим элементом EnumItem</summary>
		//////<remarks>Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Sub FillCBOWithEnumContainers(Of T)(CBO As ComboBox, ByVal EnumItem As T)
		//Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)

		With CBO
		.DataSource = Nothing
		Call .Items.Clear()
		var aContainers = EnumItem.ExtEnum_GetAllValuesAsEnumContainers
		.DataSource = aContainers


		var iFound = .ExtEnum_IndexOfEnumContainerForItem(EnumItem)
		if (iFound > 0) {
		.SelectedIndex = iFound
		}

		End With
		End Sub
		//////<summary>Заполняет ComboBox лбъектами типа <see cref="My.UOM.EnumTools.EnumContainer"/>, и выбирает текущим элементом EnumItem</summary>
		//////<remarks>Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Sub FillCBOWithEnumContainers(Of T)(CBO As ToolStripComboBox, ByVal EnumItem As T)
		//Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)

		Call CBO.ComboBox.FillCBOWithEnumContainers(EnumItem)
		End Sub

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function SelectItemWithEnumContainers(Of T)(CBO As ComboBox, EnumItem As T) As Integer
		var I = CBO.ExtEnum_IndexOfEnumContainerForItem(Of T)(EnumItem)
		if(I >= 0) { CBO.SelectedIndex = I
		return I
		End Function



	# End Region




		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_IsFlagSet(Of TEnum As Structure)(rEnumValue As TEnum,
				 EnumFlag As TEnum) As Boolean

		var iEnumValue = System.Convert.ToInt32(rEnumValue)
		var iEnumFlag = System.Convert.ToInt32(EnumFlag)
		return iEnumValue. e_IsBitsSetByMask(iEnumFlag)
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SetFlag(Of TEnum As Structure)(rEnumValue As TEnum,
			   EnumFlagToSet As TEnum,
			   Optional ByVal bSet As bool  = true) As TEnum

		var iEnumValue = System.Convert.ToInt32(rEnumValue)
		var iEnumFlag = System.Convert.ToInt32(EnumFlagToSet)

		iEnumValue = iEnumValue. e_SetBitsByMask(iEnumFlag)
		var objValue As Object = iEnumValue
		return DirectCast(objValue, TEnum)
		End Function



	# Region "OLD"


		//    //////<summary>Возвращает значение атрибута<see cref="System.ComponentModel.DescriptionAttribute"/> </summary>    
		//    <System.Runtime.CompilerServices.Extension()>
		//    Friend Function ExtEnum_GetFieldDescription(E As [Enum],
		//                                             Optional AttributeNotFoundDefaultValue As string = null) As String

		//        return Enum_GetFieldDescription(E.typeof, E.ToString, AttributeNotFoundDefaultValue)
		//    End Function




		//    //////<summary>Возвращает значение атрибута<see cref="System.ComponentModel.DescriptionAttribute"/> </summary>    
		//    <System.Runtime.CompilerServices.Extension()>
		//    Friend Function ExtEnum_GetFieldDescription(EnumType As Type,
		//                                             EnumValueString As String,
		//                                             Optional AttributeNotFoundDefaultValue As string = null) As String


		//        var T = EnumType
		//        var aEnumTypeFields = T.GetFields().ToArray //Все значения этого ENUMA

		//        var aEnumFieldsInValue = (From FLD In aEnumTypeFields Where EnumValueString.Contains(FLD.Name)).ToArray
		//        if Not (aEnumFieldsInValue.Any) {
		//            //Ни одно из полей типа ENUM не входит в данную строку!
		//            return AttributeNotFoundDefaultValue
		//        }


		//        var F = aEnumFieldsInValue.First
		//        var aDA = F.GetCustomAttributes(typeof(DescriptionAttribute), true).ToArray
		//        if (Not aDA.Any) {

		//#if DEBUG {
		//            var MSG = string.Format("!!! Enum_GetFieldDescription, для поля: //{0}// (типа: //{1}//) нет поля //System.ComponentModel.DescriptionAttribute//!",
		//                                    EnumValueString,
		//                                    T.ToString)

		//            Call DEBUG_SHOW_LINE(MSG)
		//            //Throw New Exception(MSG)
		//#}
		//            return AttributeNotFoundDefaultValue
		//        }

		//        var rFirst = aDA.First
		//        var DA = DirectCast(rFirst, DescriptionAttribute)
		//        return DA.Description
		//        }
		//        return AttributeNotFoundDefaultValue
		//    End Function


	# End Region




		////// <summary>Разделяем составной тип ENUM (Собранный через OR) на флаги</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SplitToFlags(ByVal E As[Enum]) As[Enum] ()
		var aResult() As[Enum] = {}
		if(E IsNot Nothing) {
		var T = E.typeof
		var aTypeValues = [Enum].GetValues(T)

		//if CLng(E) = 0 {
		//Нулевое значение
		//}else{
		aResult = (From rTypeValue As[Enum] In aTypeValues.Cast(Of[Enum])()
		Where(E.HasFlag(rTypeValue))
		Select rTypeValue).ToArray
		}
		return aResult
		End Function

		////// <summary>Разделяем составной тип ENUM (Собранный через OR) на флаги</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SplitToFlagsAsStrings(ByVal E As[Enum]) As String
		var aResult = ExtEnum_SplitToFlags(E)
		var sbResult = ""
		For Each V In aResult
		sbResult &= V.ToString
		sbResult &= " "
		Next
		sbResult = sbResult.Trim
		return sbResult
		End Function


		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetAllValuesAsNumericFlags(ByVal EnumType As System.Type) As Long
		var Arr = EnumType.GetEnumValues // [Enum].GetValues(EnumType)
		var TotalValue As Long = 0
		For Each EnumItemValue As Long In Arr
		TotalValue = (TotalValue Or EnumItemValue)
		Next EnumItemValue
		return TotalValue
		End Function

		////// <summary>НЕ ИСПОЛЬЗОВАТЬ вот так: typeof(XXX).EnumGetAllValuesArray</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetAllValuesArray(Of T)(EnumItem As T) As T()

		//***ВОТ ТАК РАБОТАЕТ!
		//var RR As ACCEPT_STATE_CONTAINER.eAcceptStates = ACCEPT_STATE_CONTAINER.eAcceptStates.NotApprovedAndNotDeclined
		//var eAA1 = RR.EnumGetAllValuesArray

		//*** А ВОТ ТАК НЕ БУДЕТ РАБОТАТЬ!!! 
		//var eAA2 = typeof(XXX).EnumGetAllValuesArray

		var TT As System.Type = EnumItem.typeof
		var objArr = TT.GetEnumValues
		var tArr = objArr.Cast(Of T)()
		return tArr.ToArray
		End Function

		////// <summary>Пример использования: var aContainers = DayOfWeek.Friday.EnumGetAllValuesAsEnumContainers()</summary>
		////// <returns>Возвращает массив EnumContainer(Of T)()</returns>
		////// <remarks>НЕ ИСПОЛЬЗОВАТЬ вот так: typeof(XXX).EnumGetAllValuesAsEnumContainers</remarks>
		<DebuggerNonUserCode, DebuggerStepThrough> <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetAllValuesAsEnumContainers(Of T)(ByVal EnumItem As T) As My.UOM.EnumTools.EnumContainer(Of T)()
		var aEnumValues = ExtEnum_GetAllValuesArray(Of T)(EnumItem)

		var aContainers = (From Val In aEnumValues
		Let B = New My.UOM.EnumTools.EnumContainer(Of T)(Val)
		Select B).ToArray

		return aContainers

		//***ВОТ ТАК РАБОТАЕТ!

		//Private Enum FILE_LOG_RECORDS_GROUPING As Integer
		//    <DescriptionAttribute("Имя файла")> ByFile
		//    <DescriptionAttribute("GUID")> ByGUID
		//    <DescriptionAttribute("Вызвавший процесс")> ByCaller
		//End Enum

		//var aContainers = FILE_LOG_RECORDS_GROUPING.ByCaller.EnumGetAllValuesAsEnumContainers





		//*** А ВОТ ТАК НЕ БУДЕТ РАБОТАТЬ!!! 
		//var eAA2 = typeof(XXX).EnumGetAllValuesArray
		End Function





		End Module




				Namespace My.UOM.EnumTools

				////// <summary>
				////// Используется для задания описания к полю ENUMa, непосредственно в коде (у свойства) 
				////// или если этот енум определён не нами и нельзя в каждому его полю добавить  <see cref="DescriptionAttribute"/> для ENUM
				////// Пример применения см. в самом начале класса...
				////// </summary>
				<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Enum Or AttributeTargets.Field, AllowMultiple:=true)>
				Friend Class Description2Attribute
					Inherits System.Attribute

					Public ReadOnly Property Value As Object = Nothing
					Public ReadOnly Property Description As string = Nothing

					Sub New(ByVal EnumElement As Object, ByVal ElementDescription As String)
						MyBase.New()

						Me.Value = EnumElement
						Me.Description = ElementDescription
					End Sub
				End Class

	# Region "EnumTypeConverter"
				Friend Class EnumTypeConverter
					Inherits System.ComponentModel.EnumConverter
					Public Sub New(ByVal type As System.Type)
						MyBase.New(type)
					End Sub

					Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext,
														ByVal culture As System.Globalization.CultureInfo,
														ByVal value As Object, ByVal destinationType As System.Type) As Object


						if(value IsNot Nothing) AndAlso destinationType.Equals(typeof(System.string)) {
							//Требуется приеобразовать значение в строку

							//var sEnumValueForConvertToString As string = value.ToString

							////Проверяем что есть атрибут EnumCommentAttribute
							//if (context IsNot Nothing) _
							//    AndAlso (context.PropertyDescriptor IsNot Nothing) _
							//    AndAlso (context.Instance IsNot Nothing) {

							//    var sPropertyName = context.PropertyDescriptor.Name
							//    var rObjectRef = context.Instance

							//    var aMI() = rObjectRef.typeof.GetMember(sPropertyName)
							//    For Each rMemberInfo In aMI
							//        var aEnumCommentAttributes() = CType(System.Attribute.GetCustomAttributes(rMemberInfo, typeof(EnumCommentAttribute)), EnumCommentAttribute())
							//        if (aEnumCommentAttributes.Any) {
							//            For Each ATTR In aEnumCommentAttributes
							//                if ATTR.Value.Equals(value) {
							//                    //Нашли!
							//                    return ATTR.Comment
							//                }
							//            Next
							//        }
							//    Next rMemberInfo
							//}

							////У свойства объекта нет атрибута Проверяем что у самого элемента ENUMа есть атрибут System.ComponentModel.DescriptionAttribute
							//var FI As System.Reflection.FieldInfo = value.typeof().GetField(sEnumValueForConvertToString)
							//if (FI IsNot Nothing) {
							//    var aDescriptionAttributes() = CType(FI.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false), System.ComponentModel.DescriptionAttribute())
							//    if (aDescriptionAttributes IsNot Nothing) AndAlso (aDescriptionAttributes.Any) {
							//        return (aDescriptionAttributes.First.Description)
							//    }
							//}
							//var V = GetComment(value)
							//return V
							////У элемента нет атрибута EnumDisplayNameAttribute


							var E = CType(value, [Enum])

							//Сначала пытаемся прочитать System.ComponentModel.DescriptionAttribute
							var sDescription = E.ExtEnum_GetDescriptionValue(null)
							if (sDescription. e_IsNullOrWhiteSpace) { //DescriptionAttribute не найден!
								//Читаем EnumCommentAttribute
								sDescription = ExtEnum_GetDescription2Value(E)
							}
							return sDescription
						}
						return MyBase.ConvertTo(context, culture, value, destinationType)
					End Function


					Public Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext,
														  ByVal culture As System.Globalization.CultureInfo,
														  ByVal value As Object) As Object
						Try
							if(Me.EnumType IsNot Nothing) _
							   AndAlso(value IsNot Nothing) _
							  AndAlso(value.typeof.Equals(typeof(System.string))) { //Требуется преобразовать значение из строки

								var sValue = CStr(value) //строка для которой надо найти значение ENUMа
								var ValueType = Me.EnumType //Получаем тип нашего ENUMа

								if(Not System.ComponentModel.TypeDescriptor.GetConverter(ValueType).IsValid(value)) {
									//в самом ENUMе нету элемента с таким именем! надо искать по строке-описанию!


									//Проверяем что у редакритуемого свойства есть атрибут EnumCommentAttribute
									if(context IsNot Nothing) _
									   AndAlso(context.PropertyDescriptor IsNot Nothing) _
									  AndAlso(context.Instance IsNot Nothing) {

									 var rObjRef = context.Instance
									 var sPropertyName As string = context.PropertyDescriptor.Name

									 var aMI() = rObjRef.typeof.GetMember(sPropertyName)
									 For Each MI In aMI
											var aEnumCommentAttributes() = CType(System.Attribute.GetCustomAttributes(MI, typeof(Description2Attribute)), Description2Attribute())
											if(aEnumCommentAttributes IsNot Nothing) AndAlso(aEnumCommentAttributes.Any) {
											  For Each ATTR As Description2Attribute In aEnumCommentAttributes
													if ATTR.Description.ToUpper.Equals(sValue.ToUpper) { //Нашли!
														//Получаем все имена элементов нашего ENUMа текстом
														For Each oEnumItem As Object In[Enum].GetValues(ValueType)
															//Throw New NotImplementedException
															if ATTR.Value.Equals(oEnumItem) {
																return oEnumItem
															}
														Next oEnumItem
													}
												Next
											}
										Next
									}

									//Получаем все имена элементов нашего ENUMа текстом
									var aEnumNames() = [Enum].GetNames(ValueType)
									//Для каждого из значений ENUMа смотрим есть ли у него аттрибут System.ComponentModel.DescriptionAttribute с переданной нам строкой
									For Each sEnumItem In aEnumNames
										var FI As System.Reflection.FieldInfo = ValueType.GetField(sEnumItem)
										var aDescriptionAttributes() = CType(FI.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false), System.ComponentModel.DescriptionAttribute())
										if(aDescriptionAttributes IsNot Nothing) AndAlso(aDescriptionAttributes.Any) {
										  var sEnumItemDisplayName As string = aDescriptionAttributes(0).Description
										  if sEnumItemDisplayName.Equals(sValue) {
												//Нашли элемент в ENUM с таким атрибутом
												return MyBase.ConvertFrom(context, culture, sEnumItem)
											}
										}
									Next sEnumItem
									//ни у одного элемента данного ENUMа нет атрибута EnumDisplayNameAttribute с таким значением
								}
							}

						Catch ex As Exception
	#if DEBUG {
							ex.FIX_ERROR(true)
	#}
						End Try
						return MyBase.ConvertFrom(context, culture, value)
					End Function



					////// <summary>Возвращает значение атрибута <see cref="Description2Attribute"/> для ENUM</summary>
					Private Shared Function ExtEnum_GetDescription2Value(ByVal V As[Enum],
																			  Optional ByVal MultiEnumSplitter As string = ", ") As String
						var ET = V.typeof
						if([Enum].IsDefined(ET, V)) {
							//Это одиночное значение ENUMа, определённое в нём, а не составленное из нескольких значений через OR

							var AAA = ET.GetCustomAttributes(typeof(Description2Attribute), false)
							var aComments = AAA.Cast(Of Description2Attribute)().ToArray
							if(aComments.Any) {
							   var rComment = (From EnumValue In aComments Where(EnumValue.Value.Equals(V))).ToArray
							  if(rComment.Any) {
								 var Atr = rComment.First
								 return Atr.Description
							 }
						 }

	#if DEBUG {
							var MSG = string.Format("!!! Enum_GetEnumCommentAttributeValue, для одиночного значения: //{0}// (тип: //{1}//) нет поля //EnumCommentAttribute//! - используем текстовое значение .ToString = //{0}//",
													V.ToString,
													VType.ToString)

							Call DEBUG_SHOW_LINE(MSG)
	#}

						}else{ //Это составной тип ENUM (Собранный через OR)
							var aEnumFlags = ExtEnum_SplitToFlags(V)
							if(aEnumFlags.Any) {
							   var A = (From E In aEnumFlags
										Let S = ExtEnum_GetDescription2Value(E)
										 Select S).ToArray

								var B = A. e_Join(MultiEnumSplitter)
								return B
							}


	#if DEBUG {
							var MSG = string.Format("!!! Enum_GetEnumCommentAttributeValue, для систавного значения: //{0}// (тип: //{1}//) нет полей //EnumCommentAttribute//! - используем текстовое значение .ToString = //{0}//",
													V.ToString,
													VType.ToString)

							Call DEBUG_SHOW_LINE(MSG)
	#}

						}


						return V.ToString
					End Function




				End Class
	# End Region

				////// <summary>Класс-обёртка для значений типа ENUM, для которых надо в комбобоксах и т.п. возвращать не значение, а текстовое описание...</summary>
				<DefaultProperty("Value")>
				Friend Class EnumContainer(Of T)

					Public ReadOnly[Value] As T
					Public ReadOnly ElementDescription As string = null

					Sub New(V As T, Optional sUserDescription As string = null)

						MyBase.New()
						Me.Value = V


						if (sUserDescription. e_IsNullOrWhiteSpace) {
							Me.ElementDescription = Me.Value.ToString

							//Извлекаем...
							Try
								var TT = typeof(T)
								//var E As [Enum] = CType(Me.Value, [Enum])
								//var S = ExtEnum_GetCommentAttributeValue(Me.Value)

								var sValue = Me.Value.ToString

								var OOO As Object = V
								var VVV As[Enum] = CType(OOO, [Enum])
								var S = ExtEnum_GetDescriptionValue(VVV)
								if (S. e_IsNOTNullOrWhiteSpace) { Me.ElementDescription = S
							Catch : End Try

						}else{ //Вручную указано конкретное описание
							Me.ElementDescription = sUserDescription
						}
					End Sub

					Public Overrides Function ToString() As String
						return Me.ElementDescription
					End Function


				End Class

			End Namespace
			 */


		}


		/// <summary>Network Extensions</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_DateTime
		{
			internal const string CS_DATETIME_YEAR = "yyyy";
			internal const string CS_DATETIME_MONTH_NUM = "MM";
			internal const string CS_DATETIME_MONTH_NAME = "MMM";
			internal const string CS_DATETIME_DAY = "dd";
			internal const string CS_DATETIME_HOUR = "HH";
			internal const string CS_DATETIME_MINUTE = "mm";
			internal const string CS_DATETIME_SECONDS = "ss";
			internal const string CS_DATETIME_SECONDSFRACTION = "ffff";
			internal const string C_FMT_DATETIME_LONG = "dd MMM yyyy, HH:mm:ss";
			internal const string C_FMT_DATETIME_LONGTIMESTAMP = "HH:mm:ss.fff";
			internal const string C_FMT_DATETIME_LONGDATETIMESTAMP = "dd MMM yyyy, " + C_FMT_DATETIME_LONGTIMESTAMP;
			internal const string C_FMT_DATETIME_LONGFILEDATETIMESTAMP = "yyyy-MM-dd__HH-mm-ss-fff";

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ReplaceDateTimePattern(this string SourceString, DateTime DateToInsert)
			{
				SourceString = SourceString.Replace(CS_DATETIME_YEAR, DateToInsert.Year.ToString().PadLeft(CS_DATETIME_YEAR.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_MONTH_NUM, DateToInsert.Month.ToString().PadLeft(CS_DATETIME_MONTH_NUM.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_DAY, DateToInsert.Day.ToString().PadLeft(CS_DATETIME_DAY.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_HOUR, DateToInsert.Hour.ToString().PadLeft(CS_DATETIME_HOUR.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_MINUTE, DateToInsert.Minute.ToString().PadLeft(CS_DATETIME_MINUTE.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_SECONDS, DateToInsert.Second.ToString().PadLeft(CS_DATETIME_SECONDS.Length, '0'));
				SourceString = SourceString.Replace(CS_DATETIME_SECONDSFRACTION, DateToInsert.Millisecond.ToString().PadLeft(CS_DATETIME_SECONDSFRACTION.Length, '0'));
				return SourceString;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DateTime e_RemoveSeconds(this DateTime DT) => new(DT.Year, DT.Month, DT.Day, DT.Hour, DT.Minute, 0);


			/// <summary>Следующий День</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DateTime e_NextDay(this DateTime DT) => DT.Date.AddDays(1d);


			/// <summary>"dd MMM yyyy, HH:mm:ss"</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToLongDateTimeString(this DateTime DT) => DT.ToString(C_FMT_DATETIME_LONG);


			/// <summary>"dd MMM yyyy, HH:mm:ss.fff"</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToLongDateTimeStamp(this DateTime DT) => DT.ToString(C_FMT_DATETIME_LONGDATETIMESTAMP);


			/// <summary>"HH:mm:ss.fff"</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToLongTimeStamp(this DateTime DT) => DT.ToString(C_FMT_DATETIME_LONGTIMESTAMP);


			/// <summary>"yyyy-MM-dd__HH-mm-ss-fff"</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToFileName(this DateTime DT) => DT.ToString(C_FMT_DATETIME_LONGFILEDATETIMESTAMP).Trim();


			#region To MS Access Format

			internal enum AccessTimeModes
			{
				ВзятьИзДаты,
				НачалоПериода,
				КонецПериода
			}
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_ToAccessFormat(this DateTime SourceData, AccessTimeModes Mode = AccessTimeModes.ВзятьИзДаты)
			{
				SourceData = Mode switch
				{
					AccessTimeModes.НачалоПериода => SourceData.Date,
					AccessTimeModes.КонецПериода => new DateTime(SourceData.Year, SourceData.Month, SourceData.Day, 23, 59, 59),
					_ => throw new NotImplementedException(),
				};

				return string.Format("#{1}/{0}/{2} {3}:{4}:{5}#",
					SourceData.Day.ToString("00"),
					SourceData.Month.ToString("00"),
					SourceData.Year.ToString("0000"),
					SourceData.Hour.ToString("00"),
					SourceData.Minute.ToString("00"),
					SourceData.Second.ToString("00"));
			}
			#endregion





		}


		/// <summary>Network Extensions</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_IO
		{

			/// <summary>Just test file exist to throw error if not.</summary>
			/// <exception cref="System.IO.FileNotFoundException"></exception>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_throwIfNotExist(this string path)
			{
				var fsi = path.e_ToFileSystemInfo();
				if (!fsi!.Exists) throw new System.IO.FileNotFoundException(null, path);
				//var atr = System.IO.File.GetAttributes(path);
				//return !(atr.HasFlag(FileAttributes.Directory));
			}

			/// <summary>"Add \\?\ to start of string</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_PathAddLongPathPrefix(this string sPath)
			{
				if (OSInfo.IsOSPlatform_Windows && sPath != null && !sPath.StartsWith(uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH)) sPath = uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH + sPath;
				return sPath!;
			}


			/// <summary>"Remove \\?\ from start of string</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_PathRemoveLongPathPrefix(this string sPath)
			{
				if (OSInfo.IsOSPlatform_Windows && (sPath != null) && sPath!.StartsWith(uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH)) sPath = sPath.Substring(I_O.CS_PATH_PREFIX_WIN_LONG_PATH.Length);
				return sPath!;
			}

			/// <summary>"Remove \\?\ from start of string</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_FullName_RemoveLongPathPrefix(this FileSystemInfo fsiPath) => fsiPath.FullName.e_PathRemoveLongPathPrefix();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DirectoryInfo e_ToDirectoryInfo(this string sPath, bool AddLongPathSupportIfNeed = false)
			{
				if (AddLongPathSupportIfNeed) sPath = sPath.e_PathAddLongPathPrefix();
				return new DirectoryInfo(sPath);
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileInfo e_ToFileInfo(this string sPath, bool AddLongPathSupportIfNeed = false)
			{
				if (AddLongPathSupportIfNeed) sPath = sPath.e_PathAddLongPathPrefix();
				return new FileInfo(sPath);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileStream e_CreateStreamW(this string file,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite)
				=> new(file, fm, fa, fs);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileStream e_CreateStreamW(this FileInfo fi,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite)
				=> fi.FullName.e_CreateStreamW(fm, fa, fs);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileStream e_CreateStreamR(this string file,
				FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite)
				=> new(file, fm, fa, fs);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileStream e_CreateStreamR(this FileInfo fi,
				FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite)
				=> new(fi.FullName, fm, fa, fs);





			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamReader e_CreateReader(this Stream S, bool detectEncodingFromByteOrderMarks = true) => new(S, detectEncodingFromByteOrderMarks);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamReader e_CreateReader(this string file,
			   FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite, bool detectEncodingFromByteOrderMarks = true)
				=> file.e_CreateStreamR(fm, fa, fs).e_CreateReader(detectEncodingFromByteOrderMarks);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamReader e_CreateReader(this FileInfo fi,
			  FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite, bool detectEncodingFromByteOrderMarks = true)
			   => fi.FullName.e_CreateReader(fm, fa, fs, detectEncodingFromByteOrderMarks);






			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamWriter e_CreateWriter(this FileStream fs, Encoding? enc = null, bool? AutoFlush = true)
			{
				enc ??= Encoding.Unicode;
				var sw = new StreamWriter(fs, enc);
				if (null != AutoFlush) sw.AutoFlush = AutoFlush.e_ToBool();
				return sw;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamWriter e_CreateWriter(this string file,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite,
				Encoding? enc = null, bool? AutoFlush = true)
				=> file.e_CreateStreamW(fm, fa, fs).e_CreateWriter(enc, AutoFlush);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StreamWriter e_CreateWriter(this FileInfo fi,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite,
				Encoding? enc = null, bool? AutoFlush = true)
					=> fi.FullName.e_CreateWriter(fm, fa, fs, enc, AutoFlush);


			/// <summary>Multiplatform FileAttributes.ReparsePoint
			/// The file contains a reparse point, which is a block of user-defined data associated with a file or a directory. 
			/// ReparsePoint is supported on Windows, Linux, and macOS.
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNTFS_SymLinkMP(this FileSystemInfo fsi) => fsi.Attributes.HasFlag(FileAttributes.ReparsePoint);


			/// <summary>Multiplatform FileAttributes.ReparsePoint
			/// The file contains a reparse point, which is a block of user-defined data associated with a file or a directory. 
			/// ReparsePoint is supported on Windows, Linux, and macOS.
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsNTFS_SymLinkMP(this string path) => path.e_ToFileSystemInfo()!.e_IsNTFS_SymLinkMP();




#if NET6_0

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string e_SymLinkToString(this FileSystemInfo fsi)
			{
				if (!fsi.e_IsNTFS_SymLinkMP()) return fsi.ToString();
				var sMsg = $"SymLink '{fsi.e_FullName_RemoveLongPathPrefix()}'";
				try
				{
					var sSymLinkTarget = fsi.LinkTarget;
					if (sSymLinkTarget != null) sMsg += $" => '{sSymLinkTarget}'";
				}
				catch { }
				return sMsg;
			}
#endif



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Uri e_ToURI(this string SourceText) => new(SourceText);





			/// <summary>Use UOM.Win32.Shell.PathIsDirectory</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileSystemInfo? e_ToFileSystemInfo(this string? sPath)
				=> sPath.e_IsNullOrWhiteSpace() ? null : (sPath!.e_IsDirectory()) ? sPath!.e_ToDirectoryInfo() : sPath!.e_ToFileInfo();



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo? e_ToFileInfo(this string? sPath) => (sPath.e_IsNullOrWhiteSpace() ? null : new(sPath!));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo[] e_ToFileInfo(this IEnumerable<string> files)
				=> files.Where((S) => S.e_IsNOTNullOrWhiteSpace()).Select<string, FileInfo>(S => S.e_ToFileInfo()!).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo[] e_ToFileInfo(this IEnumerable<FileSystemInfo> files) => files.Cast<FileInfo>().ToArray();











			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo? e_ToDirectoryInfo(this string? sPath) => (sPath.e_IsNullOrWhiteSpace() ? null : new(sPath!));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo[] e_ToDirectoryInfo(this IEnumerable<string> aDirs)
			=> aDirs.Where((S) => S.e_IsNOTNullOrWhiteSpace()).Select<string, DirectoryInfo>(S => S.e_ToDirectoryInfo()!).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo[] e_ToDirectoryInfo(this IEnumerable<FileSystemInfo> afsoFiles) => afsoFiles.Cast<DirectoryInfo>().ToArray();

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo e_Parent(this FileSystemInfo fsi)
				=> (fsi is DirectoryInfo di)
				? di.Parent!
				: ((FileInfo)fsi).Directory!;



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileSystemInfo e_FindFirstExisting(this string path)
			{
				FileSystemInfo fsi = new FileInfo(path);
				while (!fsi.Exists)
				{
					//If the path does not exist, we jump up to the parent
					fsi = fsi.e_Parent();
					if (null == fsi) throw new FileNotFoundException(null, path);
				}
				return fsi;
			}




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string[] e_ToStrings(this IEnumerable<FileSystemInfo> aFSI)
			=> aFSI.Select<FileSystemInfo, string>((fsi) => fsi.FullName).ToArray();







			#region Read...


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ReadAllBytes(this string sFile) => File.ReadAllBytes(sFile);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static MemoryStream e_ReadToMemory(this string sFile) => new(sFile.e_ReadAllBytes());


			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static FileStream e_OpenRead(this string sFile, FileAccess Access = FileAccess.Read, FileShare Share = FileShare.None)
			//{
			//    return new FileStream(sFile, FileMode.Open, Access, Share);
			//}

			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static FileStreame_OpenRead(this FileInfo FI, FileAccess Access = FileAccess.Read, FileShare Share = FileShare.None)
			//{
			//    return FI.FullName.e_OpenRead(Access, Share);
			//}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReadAllText(this string Path, Encoding? ENC = null) => File.ReadAllText(Path, ENC ?? Encoding.Unicode);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string[] e_ReadAllLines(this string Path, Encoding? ENC = null, bool SkipEmptyLines = false)
			{
				var aLines = Array.Empty<string>();
				if (null == ENC)
					aLines = File.ReadAllLines(Path);
				else
					aLines = File.ReadAllLines(Path, ENC);

				if (SkipEmptyLines) aLines = (from sLine in aLines
											  where sLine.e_IsNOTNullOrWhiteSpace()
											  select sLine).ToArray();
				return aLines;
			}
			#endregion







			/// <summary>Create Backup copy of file. (COPY or MOVE) Return BackUp File</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string? e_MakeBackUpIfExist(this string sFile, bool MoveInsteadOfCopy = false)
			{
				if (!File.Exists(sFile)) return null;

				string sPathBAK = $"{sFile}.{DateTime.Now.e_ToFileName()}.bak.";
				if (File.Exists(sPathBAK)) File.Delete(sPathBAK);

				if (MoveInsteadOfCopy) File.Move(sFile, sPathBAK);
				else File.Copy(sFile, sPathBAK);
				return sPathBAK;
			}

			/// <summary>Create Backup copy of file. (COPY or MOVE) Return BackUp File</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileInfo? e_MakeBackUpIfExist(this FileInfo File, bool MoveInsteadOfCopy = false)
			{
				var sBackup = File.FullName.e_MakeBackUpIfExist(MoveInsteadOfCopy);
				return sBackup.e_IsNOTNullOrWhiteSpace() ? sBackup.e_ToFileInfo() : null;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_CreateIfNotExist(this DirectoryInfo DI)
			{ if (!DI.Exists) DI.Create(); }


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_CreateIfNotExist(this string sDirectoryPath)
			{ if (!Directory.Exists(sDirectoryPath)) Directory.CreateDirectory(sDirectoryPath); }


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_DeleteIfExist(this DirectoryInfo FI)
			{ if (FI.Exists) FI.Delete(); }


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_DeleteIfExist(this string sFilePath)
			{ if (File.Exists(sFilePath)) File.Delete(sFilePath); }












			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo e_GetExeWithDifferentExt(this string NewExt)
				=> new(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, NewExt));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo e_GetFileIn_SpecialFolder(this string FileName, Environment.SpecialFolder SF)
				=> new(Path.Combine(Environment.GetFolderPath(SF), FileName));


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo e_GetFileIn_SpecialFolder_Desktop(this string FileName)
				=> FileName.e_GetFileIn_SpecialFolder(Environment.SpecialFolder.DesktopDirectory);




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo e_GetFileIn_TempDir(this string FileName)
				=> new(Path.Combine(Path.GetTempPath(), FileName));






			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static char[] e_ReadAllChars(this StreamReader SR)
			{
				var lBuffer = new List<char>();
				var buffer = new char[1025];
				int iReadCount;
				do
				{
					iReadCount = SR.Read(buffer, 0, buffer.Length);
					if (iReadCount > 0)
					{
						var abData = new char[iReadCount];
						Array.Copy(buffer, abData, iReadCount);
						lBuffer.AddRange(abData);
					}
				}
				while (iReadCount > 0);
				var aChars = lBuffer.ToArray();
				return aChars;
			}




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_ReadAllCharsAsString(this StreamReader SR) => new(SR.e_ReadAllChars());



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_GetFileNameWithoutExtension(this FileInfo FI) => Path.GetFileNameWithoutExtension(FI.Name);




			/// <summary>Use UOM.Win32.Shell.PathIsDirectory</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsDirectory(this FileSystemInfo Path) => Path.FullName.e_IsDirectory();


			/// <summary>Check (TypeOf FSI Is DirectoryInfo)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsDirectoryInfo(this FileSystemInfo FSI) => FSI is DirectoryInfo;


			/// <summary>Check (TypeOf FSI Is DirectoryInfo)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsFileInfo(this FileSystemInfo FSI) => FSI is FileInfo;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo[] e_TakeAllDirectories(this IEnumerable<FileSystemInfo> aFSI)
				=> (from DI in aFSI.Where(fsi => fsi.e_IsDirectoryInfo()).Select<FileSystemInfo, DirectoryInfo>(fsi => (DirectoryInfo)fsi)
					orderby DI.Name
					select DI).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo[] e_TakeAllFiles(this IEnumerable<FileSystemInfo> aFSI)
				=> (from FI in aFSI.Where(fsi => !fsi.e_IsDirectoryInfo()).Select<FileSystemInfo, FileInfo>(fsi => fsi.e_ToFileInfo())
					orderby FI.Name
					select FI).ToArray();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo e_ToFileInfo(this FileSystemInfo fsi) => (FileInfo)fsi;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo e_GetChildDir(this DirectoryInfo diParent, string sChildDirName)
			{
				var aChild = diParent.GetDirectories(sChildDirName);
				if (aChild.Length != 1) throw new Exception($"GetDirectories({sChildDirName}) = {aChild.Length}");
				return aChild.First();
			}



			/// <summary>Use UOM.Win32.Shell.PathIsDirectory</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsDirectory(this string sPath)
				=> System.IO.Directory.Exists(sPath) && new System.IO.DirectoryInfo(sPath).Attributes.HasFlag(FileAttributes.Directory);





			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static FileInfo[] e_GetFilesSorted(this DirectoryInfo DI, string? searchPattern = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
			{
				searchPattern ??= "*.*";

				var aFiles = DI.GetFiles(searchPattern, searchOption);
				aFiles = (from FI in aFiles
						  orderby FI.Name ascending, FI.FullName ascending
						  select FI).ToArray();

				return aFiles;
			}







			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ReadAllBytes(this Stream SM)
			{
				if (SM.Length < 1L) return Array.Empty<byte>();
				SM.Seek(0L, SeekOrigin.Begin);
				var abData = new byte[((int)SM.Length)];
				SM.Read(abData, 0, abData.Length);
				return abData;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static (byte[] Buffer, int ReadBytesCount) e_Read(this Stream SM, int iCount, int Offset = 0)
			{
				var abBuffer = new byte[iCount];
				var iRead = SM.Read(abBuffer, Offset, iCount);
				return (abBuffer, iRead);
			}






























		}

		/// <summary>Network Extensions</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_Network
		{

			public static async void e_DownloadFile(this string FileUrl,
				Action<System.IO.Stream> cbNetworkStreamAction,
				int iTimeout = 30000)
			{
#if NET40
        WebClient _client = new WebClient();
              HttpWebRequest? webRequest = WebRequest.Create(FileUrl) as HttpWebRequest;

 HttpWebRequest? webRequest = WebRequest.Create(FileUrl) as HttpWebRequest;
            webRequest!.AllowWriteStreamBuffering = true;
            webRequest.Timeout = iTimeout;
            webRequest.ServicePoint.ConnectionLeaseTimeout = 5000;
            webRequest.ServicePoint.MaxIdleTime = 5000;

            try
            {
                using (WebResponse webResponse = webRequest!.GetResponse())
                {
                    var tskGetStream = new Task<System.IO.Stream>(() => webResponse.GetResponseStream());

                    //    .GetAwaiter().GetResult();




                    using (System.IO.Stream stream = webResponse.GetResponseStream())
                    {
                        cbNetworkStreamAction.Invoke(stream);
                    }
                }
            }
            finally
            {
                webRequest.ServicePoint.CloseConnectionGroup(webRequest.ConnectionGroupName!);
                webRequest = null;
            }
#else
				using (var _client = new HttpClient())
				{
					_client.Timeout = TimeSpan.FromMilliseconds(iTimeout);
					//var stream = await _client.GetStreamAsync(FileUrl);
					using (var stream = await _client.GetStreamAsync(FileUrl))
					{
						cbNetworkStreamAction.Invoke(stream);
					}
				};
#endif

			}

			/// <summary>Download File From Remote Using 'HttpWebRequest'</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			public static System.IO.FileInfo? e_DownloadFile(this string FileUrl, System.IO.FileInfo? Target = null)
			{
				FileInfo? fiDownloaded = null;
				FileUrl.e_DownloadFile(stDownload =>
				{
					var fiTMP = Target ?? new FileInfo(System.IO.Path.GetTempFileName());
					try
					{
						using (var fsTMP = fiTMP.OpenWrite())
						{
							stDownload.CopyTo(fsTMP);
							fsTMP.Flush();
						}
						fiDownloaded = fiTMP;
					}
					finally
					{
						if (null == fiDownloaded) fiTMP.Delete();
					}
				});
				return fiDownloaded;
			}


			/// <summary>Download File From Remote Using 'WebClient'</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			public static (System.IO.FileInfo? DownloadedFile, AsyncCompletedEventArgs? AsyncDownloadResult)
				e_DownloadFile(this string FileUrl, System.IO.FileInfo? Target = null, Action<DownloadProgressChangedEventArgs>? cbDownloadProgress = null)
			{
				using (var evtFinished = new ManualResetEvent(false))
				{

					//https://newbedev.com/progress-bar-with-httpclient
					//using (var client = new HttpClient())
					//{
					//    client.Timeout = TimeSpan.FromMinutes(5);

					//    // Create a file stream to store the downloaded data.
					//    // This really can be any type of writeable stream.
					//    using (var file = new FileStream("sdf", FileMode.Create, FileAccess.Write, FileShare.None))
					//    {

					//        // Use the custom extension method below to download the data.
					//        // The passed progress-instance will receive the download status updates.
					//        await client.DownloadAsync(FileUrl, file, progress, cancellationToken);
					//    }
					//}


#pragma warning disable SYSLIB0014 // Type or member is obsolete
					using WebClient webClient = new();
#pragma warning restore SYSLIB0014 // Type or member is obsolete
					try
					{
						var fiBuffer = Target ?? new FileInfo(System.IO.Path.GetTempFileName());

						webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler((_, e) => cbDownloadProgress?.Invoke(e));

						AsyncCompletedEventArgs? acea = null;
						webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((_, e) =>
						{
							acea = e;
							evtFinished.Set();
						});

						webClient.DownloadFileAsync(new Uri(FileUrl), fiBuffer.FullName);
						evtFinished.WaitOne();
						return (DownloadedFile: fiBuffer, AsyncDownloadResult: acea);
					}
					finally
					{
						evtFinished.Set();
					}
				}
			}





			/// <summary>Download File From Remote Using 'WebClient' and displaying progress bar</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
			public static (System.IO.FileInfo? DownloadedFile, AsyncCompletedEventArgs? AsyncDownloadResult)
				e_DownloadFileConsole(this string FileUrl,
				System.IO.FileInfo? Target = null,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:")
			{

				try { Console.CursorVisible = false; } catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
				try
				{
					(0).e_WriteConsoleProgress(ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString);//Display Zero Progress
					var dlResult = FileUrl.e_DownloadFile(Target, (e) => e.ProgressPercentage.e_WriteConsoleProgress(ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString));
					return (dlResult.DownloadedFile, dlResult.AsyncDownloadResult);
				}
				finally
				{
					Console.WriteLine();//Out CrLf for next line output
					try { Console.CursorVisible = true; } catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
				}
			}
		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_Object
		{

			///// <summary>Clean up the COM variables</summary>
			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void  e_DisposeAndSetNothingCOMObject([MarshalAs(UnmanagedType.IUnknown)][In] this out object rCOMObject, bool ThrowExceptionOnError = false)
			//{
			//    try
			//    {
			//        while (Marshal.ReleaseComObject(rCOMObject) != 0)
			//            Application.DoEvents(); // Wait
			//        rCOMObject = null;
			//    }
			//    catch
			//    {
			//        if (ThrowExceptionOnError)
			//            throw;
			//    }
			//}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void ThrowIfNull<T>(this T? obj)
			{
				_ = obj ?? throw new ArgumentNullException(nameof(obj));
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_DisposeAndSetNothing<T>(this T ObjToDispose, bool ThrowExceptionOnError = false) where T : IDisposable
			{
				try { ObjToDispose?.Dispose(); }
				catch { if (ThrowExceptionOnError) throw; }
			}

			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void  e_DisposeAll([In()] this IEnumerable<IDisposable> T, bool ThrowExceptionOnError = false)
			//{
			//    if (T !=null)
			//    {
			//        foreach (var ObjRef in T)
			//        {
			//            if (ObjRef !=null)
			//            {
			//                try
			//                {
			//                    ObjRef.Dispose();
			//                }
			//                catch
			//                {
			//                    if (ThrowExceptionOnError)
			//                        throw;
			//                }
			//            }
			//        }
			//    }

			//    T = null;
			//}
		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_Async_MT
		{

			public static void e_RunTryCatch(
				this Func<uint> operation,
				string messageTemplate,
				params object[] messageArgs)
			{
				uint result = operation.Invoke();
				if (result != 0)
				{
					string message = string.Format(messageTemplate, messageArgs);
					throw new Exception($"{message}. Error code {result} (see WinError.h)");
				}
			}

			public static void e_RunTryCatch(
				this Func<bool> operation,
				string messageTemplate,
				params object[] messageArgs)
			{
				bool result = operation.Invoke();
				if (!result)
				{
					string message = string.Format(messageTemplate, messageArgs);
					throw new Exception($"{message}. Error code {result} (see WinError.h)");
				}
			}





			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<T>
				e_RunAsync<T>(
				this Func<T> A,
				TaskCreationOptions TaskFlags = TaskCreationOptions.LongRunning)
			{
				using (var tskAsync = new Task<T>(() => A.Invoke(), TaskFlags))
				{
					tskAsync.Start();
					return await tskAsync;
				}
			}


			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_RunSyncLock<T>(this object rLockObject, Func<T> A)
			{
				T R;
				lock (rLockObject) R = A.Invoke();
				return R;
			}


			#region TryCatch


			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_RunTryCatch(this Action? action)
			{
				_ = action ?? throw new ArgumentNullException(nameof(action));
				try
				{
					action.Invoke();
					return true;
				}
				catch { return false; }
			}


			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static TFuncResult? e_RunTryCatch<TFuncResult>(this Func<TFuncResult>? action, TFuncResult? ResultOnError)
			{
				_ = action ?? throw new ArgumentNullException(nameof(action));
				try { return action.Invoke(); }
				catch { return ResultOnError; }
			}

			#endregion

			#region TryFinally

			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<TFuncResult> e_RunTryFinally<TFuncResult>(
				this Task<TFuncResult>? FuncMain,
				Action? FuncPrefix = null,
				Action? FuncFinally = null)
			{
				_ = FuncMain ?? throw new ArgumentNullException("MainBodyFunc");

				try
				{
					FuncPrefix?.Invoke();
					return await FuncMain;
				}
				finally { FuncFinally?.Invoke(); }
			}


			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static TFuncResult e_RunTryFinally<TFuncResult>(
				this Func<TFuncResult>? FuncMain,
				Action? FuncPrefix = null,
				Action? FuncFinally = null)
			{
				_ = FuncMain ?? throw new ArgumentNullException(nameof(FuncMain));
				try
				{
					FuncPrefix?.Invoke();
					return FuncMain.Invoke();
				}
				finally { FuncFinally?.Invoke(); }
			}

			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_RunTryFinally(
				this Action? FuncMain,
				Action? FuncPrefix = null,
				Action? FuncFinally = null)
			{
				_ = FuncMain ?? throw new ArgumentNullException("MainBodyFunc");
				try
				{
					FuncPrefix?.Invoke();
					FuncMain.Invoke();
				}
				finally { FuncFinally?.Invoke(); }
			}
			#endregion

			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task e_RunAsync_WaitAllTasksFinished<T>(this IEnumerable<Task<T>> aTasks,
				bool StartAllTasks,
				Action<T> OnEachTaskCompleted)
			{
				_ = aTasks ?? throw new ArgumentNullException(nameof(aTasks));
				if (!aTasks.Any()) return;

				var lTasks = aTasks.ToList();
				if (StartAllTasks) lTasks.ForEach(TSK => TSK.Start()); // Start All Tasks

				while (lTasks.Any())
				{
					var firstFinishedTask = await Task.WhenAny(lTasks);
					lTasks.Remove(firstFinishedTask); // Remove this task from list
					var rslt = firstFinishedTask.Result;
					OnEachTaskCompleted?.Invoke(rslt);
				}
			}



			/// <summary>Exec Async Function In Sync Mode</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_RunAsyncAsSync<T>(this Task<T> tskAsync) => tskAsync.GetAwaiter().GetResult();

			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static T runAsyncAsSync<T, P1>(this Task<T> tskAsync(P1 PPP)) => tskAsync(P1).GetAwaiter().GetResult();











			/*



			private const string CS_TIMEOUT_EXPORED_FORMAT = "Не удалось выполнить операцию за {0}";

			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool ExtAsync_Wait(this EventWaitHandle EVT, int WAIT_TIMEOUT, string TimeoutMessage = CS_TIMEOUT_EXPORED_FORMAT, bool ThrowTimeoutException = true)
			{
				bool bWait = EVT.WaitOne(WAIT_TIMEOUT);
				if (!bWait)
				{
					if (ThrowTimeoutException)
					{
						string sErr = TimeoutMessage. e_Format(WAIT_TIMEOUT. e_ToShellTimeString(5));
						var EX = new TimeoutException(sErr);
						throw EX;
					}
				}

				return bWait;
			}

			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task ExtAsync_CreateEventAndWaitAsync(this string EventName, Func<bool> OnEventCallback)
			{
				using (var evtWait = new EventWaitHandle(false, EventResetMode.AutoReset, EventName))
				{
					bool bContinueWait = false;
					do
					{
						// MsgBox("START WAIT... " & C_EVENT_NAME)
						await runAsync(() => evtWait.WaitOne(Timeout.Infinite));
						// Await e_runAsync(Function() SomeLongWorkingFunction)

						bContinueWait = OnEventCallback.Invoke();
					}
					while (bContinueWait);
				}
			}

			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task ExtAsync_CreateEventAndWaitAsync(this string EventName, Task<bool> OnEventCallback)
			{
				using (var evtWait = new EventWaitHandle(false, EventResetMode.AutoReset, EventName))
				{
					bool bContinueWait = false;
					do
					{
						// MsgBox("START WAIT... " & C_EVENT_NAME)
						await runAsync(() => evtWait.WaitOne(Timeout.Infinite));
						bContinueWait = await OnEventCallback;
					}
					while (bContinueWait);
				}
			}

			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void ExtAsync_SignalNamedEvent(this string sName)
			{
				// MsgBox("OpenExisting... " & C_EVENT_NAME)
				using (var E = EventWaitHandle.OpenExisting(sName))
				{
					E.Set();
				}
				// MsgBox("Opened And Setted! " & C_EVENT_NAME)                
			}






	#region e_runAsync Await Helpers


			/// <summary>Ожидаем сигнала через AWAIT</summary>
			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task<bool> ExtAsync_WaitAsync(this WaitHandle EVT, UOM.MTSafeBooleanFlag rStopFlagToSet = null, int iTimeout = Timeout.Infinite)
			{
				return await runAsync(() =>
				{
					if (rStopFlagToSet !=null)
						rStopFlagToSet.SetlFlag(); // Ставим СТОП-ФЛАГ
					return EVT.WaitOne(iTimeout);
				});
			}

			/// <summary>Блокируем UI, взводим стоп-флаг и ждём готовности, через AWAIT</summary>
			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task<bool> ExtAsync_WaitOnLockedUIAsync(this WaitHandle EVT, Control[] aUI, UOM.MTSafeBooleanFlag rStopFlagToSet = null, int iWaitTimeout = Timeout.Infinite)
			{
				return await TryFinally(() => await EVT.ExtAsync_WaitAsync(rStopFlagToSet, iWaitTimeout), () => aUI.ExtCtl_Enable(false), () => aUI.ExtCtl_Enable(true));

	#region Old
				// Call aUI.ExtCtl_Enable(False) 'Блокируем UI
				// Try

				// Call rStopFlagToSet.SetlFlag() 'Ставим СТОП-ФЛАГ
				// Return Await EVT.ExtAsync_WaitEventAsync(iWaitTimeout) 'Ждём окончания потока

				// Finally
				// Call aUI.ExtCtl_Enable(True) 'Включаем UI
				// End Try
	#endregion
			}





	#endregion
			*/


			/// <summary>Check event Flag</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsSet(this EventWaitHandle Evt) => null != Evt && Evt.WaitOne(0, false);



			// TODO: Fix Asyncs with WinForms!

			/*


			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task<T> e_runAsync<T>(this Form F, Func<T> A, TaskCreationOptions TaskFlags = TaskCreationOptions.LongRunning, bool UseWaitCursor = true, bool CatchAndShowError = true)
			{
				try
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = true;
					return await A.e_runAsync(TaskFlags);
				}
				catch (Exception ex)
				{
					if (!CatchAndShowError)
						throw;
					ex.FIX_ERROR(true);
				}
				finally
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = false;
				}

				return default(Task<T>);
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task e_runAsync(this Form F, Task A, bool UseWaitCursor = true, bool CatchAndShowError = true)
			{
				try
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = true;
					await A;
					return;
				}
				catch (Exception ex)
				{
					if (!CatchAndShowError)
						throw;
					ex.FIX_ERROR(true);
				}
				finally
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = false;
				}
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task<T> e_runAsync<T>(this Form F, Task<T> A, TaskCreationOptions TaskFlags = TaskCreationOptions.LongRunning, bool UseWaitCursor = true, bool CatchAndShowError = true)
			{
				try
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = true;
					return await A;
				}
				catch (Exception ex)
				{
					if (!CatchAndShowError)
						throw;
					ex.FIX_ERROR(true);
				}
				finally
				{
					if (F !=null && UseWaitCursor)
						F.UseWaitCursor = false;
				}

				return default(Task<T>);
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal async static Task e_runAsync(this Form F, Action A, TaskCreationOptions TaskFlags = TaskCreationOptions.LongRunning, bool UseWaitCursor = true, bool CatchAndShowError = true)
			{
				bool fCallAction()
				{
					A.Invoke();
					return true;
				};
				var bDummy = await F.e_runAsync(fCallAction, TaskFlags, UseWaitCursor, CatchAndShowError);
				return;
			}
			*/

		}



		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_DebugAndErrors
		{
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void DEBUG_SHOW_LINE(this string sMessage) => $"{sMessage} \r\n".DEBUG_SHOW();

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void DEBUG_SHOW(this string sMessage)
			{
#if DEBUG
				Debug.Write(sMessage);
#endif
			}
		}



		internal static partial class Extensions_Handle
		{



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsValid(this IntPtr h) => ((h != IntPtr.Zero) && (h != uom.constants.HANDLE_INVALID));


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsNotValid(this IntPtr h) => !h.e_IsValid();


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsValid(this SafeHandle? sh) => (sh != null) && (!sh!.IsInvalid);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool e_IsValid(this HandleRef HR) => HR.Handle.e_IsValid();




			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function IsValid(ByVal Handle As Global.Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid) As Boolean
			// SafeHandleZeroOrMinusOneIsInvalid
			// Return Not Handle.IsInvalid
			// End Function

			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function IsValid(ByVal hFile As SafeFileHandle) As Boolean
			// Dim B = (Not hFile.IsInvalid) AndAlso (Not hFile.IsClosed)
			// Return B
			// End Function






		}




		internal static partial class Extensions_Globalization
		{


			/// <summary>Возвращает корневой элемент для дерева языков.
			/// для Ru-Ru будет RU,
			/// для EN-US будет EN</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static CultureInfo e_GetTopParent(this CultureInfo Cult)
			{
				while (Cult.Parent != null && Cult.Parent.Name.e_IsNOTNullOrWhiteSpace()) Cult = Cult.Parent;
				return Cult;
			}

			/// <summary>Это российское дерево языков. 
			/// Любой язык, где корневой элемент = RU</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool e_IsRussianTree(this CultureInfo Cult) => Cult.e_GetTopParent().LCID == 25;

		}






		//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static partial class Extensions_Reflection
		{

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_CreateInstance<T>(this Type ClassType) => (T)Activator.CreateInstance(ClassType)!;


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static object e_CreateInstance(this TypeInfo rT, object[] ConstructorArgs)
			{
				// Dim rConstructor = rT.GetConstructor(Type.EmptyTypes)
				// Dim rConstructor = rT.GetConstructor(New Type() {BTSD.GetType})
				// If (rConstructor Is Nothing) Then
				// Dim sErr = "Запуск фонового задания '{0}' не удался, не найден конструктор с параметрами '{1}'!".Trim. e_FormatWrap(rT.FullName,
				// BTSD.GetType.FullName)
				// Throw New System.Reflection.AmbiguousMatchException(sErr)
				// End If
				// Dim rInst = rConstructor.Invoke(New Object() {BTSD})


				if (ConstructorArgs.Length < 1) throw new Exception("use Typed e_CreateInstance(Of XXX) instead!");

				var aParamsTypes = (from rArg in ConstructorArgs
									let tArg = rArg.GetType()
									select tArg).ToArray();

				var rConstructor = rT.GetConstructor(aParamsTypes);
				if (null == rConstructor)
					throw new AmbiguousMatchException($"Cant find class constructor for type '{rT.FullName}' with specifed args count ('{ConstructorArgs.Length}') and arg types!");

				return rConstructor.Invoke(ConstructorArgs);
			}




			#region Копирование свойств объектов

			private const BindingFlags DEFAULT_BF = BindingFlags.NonPublic | BindingFlags.Instance;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static object? e_GetPropertyValue(this object refObj, string sPropertyName, BindingFlags BF = DEFAULT_BF)
			{
				var typLVG = refObj.GetType();
				var piID = typLVG.GetProperty(sPropertyName, BF);
				if (null == piID) throw new ArgumentOutOfRangeException($"Object '{typLVG}' does not have property '{sPropertyName}'");
				return piID.GetValue(refObj, null);

			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Int32 e_GetPropertyValue_Integer(this object refObj, string sPropertyName, BindingFlags BF = DEFAULT_BF)
			{
				var objVal = refObj.e_GetPropertyValue(sPropertyName, BF);
				return (objVal != null) ? Convert.ToInt32(objVal) : 0;
			}


			/// <summary>Копируем значения всех свойств</summary>
			/// <param name="Destination">У этого свойства заполняются данными</param>
			/// <param name="Source">У этого объекта берутся значения свойств.</param>
			/// <param name="ThrowErrorIfFieldNotFound">Вызываеть ошибку если необходимое свойство не найдено в источнике данных</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_CopyPropertyValuesFrom(this object Destination, object Source, IEnumerable<string> PropertyNames, bool ThrowErrorIfFieldNotFound)
			{
				var aPropsSrc = TypeDescriptor.GetProperties(Source);
				var aPropsDest = TypeDescriptor.GetProperties(Destination);
				$"*** Copy Properties From {Source.GetType()} to {Destination.GetType()}".DEBUG_SHOW_LINE();

				foreach (var sPropertyName in PropertyNames)
				{
					var aTarpetProperties = (from P in aPropsDest.Cast<PropertyDescriptor>()
											 where (P.Name.ToLower() ?? "") == (sPropertyName.ToLower() ?? "")
											 select P).ToArray();
					if (aTarpetProperties.Any())
					{
						var rTarpetProperty = aTarpetProperties.First();
						if (!rTarpetProperty.IsReadOnly)
						{
							var aSoupceProperty = (from P in aPropsSrc.Cast<PropertyDescriptor>()
												   where (P.Name.ToLower() ?? "") == (sPropertyName.ToLower() ?? "")
												   select P).ToArray();
							if (aSoupceProperty.Any())
							{
								var rFirstProp = aSoupceProperty.First();
								var objVal = rFirstProp.GetValue(Source);
								rTarpetProperty.SetValue(Destination, objVal);

#if DEBUG
								var sVal = "[Nothing]".ToUpper();
								var sType = sVal;
								if (null != objVal)
								{
									sType = objVal.GetType().ToString();
									sVal = objVal.ToString();
								}
								$"Copied '{sPropertyName}' = {sType}:('{sVal}')".DEBUG_SHOW_LINE();
#endif
							}
							else
							{
								// Свойство с таким именем не найдено в объекте-источнике
								string sError = string.Format("Свойство '{0}' не найдено в объекте-источнике!", sPropertyName);
#if DEBUG
								sError.DEBUG_SHOW_LINE();
#endif
								if (ThrowErrorIfFieldNotFound) throw new Exception(sError);
							}
						}
					}
				}
			}

			internal enum PROPERTIES_SOURCES : int
			{
				/// <summary>Список свойств берётся у объекта-назначения</summary>
				DestinationObject = 0,
				/// <summary>Список свойств берётся у объекта-источника</summary>
				SourceObject
			}
			/// <summary>Копируем значения всех свойств</summary>
			/// <param name="Destination">У этого объекта берётся список свойств и его свойства заполняются данными</param>
			/// <param name="Source">У этого объекта берутся только значения свойств.</param>
			/// <param name="ThrowErrorIfFieldNotFound">Вызываеть ошибку если необходимое свойство не найдено в источнике данных</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_CopyPropertyValuesFrom(this object Destination, object Source, PROPERTIES_SOURCES PropertyNamesSource, bool ThrowErrorIfFieldNotFound)
			{
				var aPropNames = PropertyNamesSource switch
				{
					PROPERTIES_SOURCES.SourceObject => (from P in TypeDescriptor.GetProperties(Source).Cast<PropertyDescriptor>() where !P.IsReadOnly select P.Name),
					PROPERTIES_SOURCES.DestinationObject => (from P in TypeDescriptor.GetProperties(Destination).Cast<PropertyDescriptor>() where !P.IsReadOnly select P.Name),
					_ => throw new ArgumentException("PropertyNamesSource")
				};
				Destination.e_CopyPropertyValuesFrom(Source, aPropNames.ToArray(), ThrowErrorIfFieldNotFound);
			}
			#endregion




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IntPtr OffsetOfField(this Type T, string FieldName)
			{
				var iOffset = Marshal.OffsetOf(T, FieldName);
				return iOffset;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int OffsetOfField32(this Type T, string FieldName)
			{
				return T.OffsetOfField(FieldName).ToInt32();
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static long OffsetOfField64(this Type T, string FieldName)
			{
				return T.OffsetOfField(FieldName).ToInt64();
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_SetPrivateFieldValue<T>(
				this T obj,
				string fieldNameStartWith,
				object newValue,
				BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy)
			{
				fieldNameStartWith = fieldNameStartWith.ToLower();

				FieldInfo[] aFields = obj!.GetType().GetFields(bFlags);
				FieldInfo? fi = aFields.Where(f => (f.Name.ToLower().StartsWith(fieldNameStartWith))).FirstOrDefault();
				fi?.SetValue(obj, newValue);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? e_GetAttributeOf<T>(this PropertyDescriptor pd) where T : System.Attribute
				=> (T?)pd.Attributes[typeof(T)];



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_SetAttributeValueOf<TAttr>(
				this PropertyDescriptor pd,
				string attributeInternalFiledStartWith,
				object newValue,
				BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy
				) where TAttr : Attribute
					=> pd.e_GetAttributeOf<TAttr>()?.e_SetPrivateFieldValue(attributeInternalFiledStartWith, newValue, bFlags);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_SetAttribute_Browsable(this PropertyDescriptor pd, bool browsable)
				=> pd.e_SetAttributeValueOf<BrowsableAttribute>("<Browsable", browsable);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_SetAttribute_ReadOnly(this PropertyDescriptor pd, bool readOnly)
				=> pd.e_SetAttributeValueOf<ReadOnlyAttribute>("<isReadOnly", readOnly);



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void PropertyGrid_SetClassPropertiesBrowsable<T>(
				this T o,
				bool browsable,
				Func<PropertyDescriptor, bool>? wherePredicate = null)
			{
				PropertyDescriptor[] aProps = TypeDescriptor.GetProperties(o!.GetType()).Cast<PropertyDescriptor>().ToArray();
				if (wherePredicate != null) aProps = aProps.Where(pd => wherePredicate(pd)).ToArray();
				aProps.e_ForEach(pd => pd.e_SetAttribute_Browsable(browsable));
			}




			/// <summary>
			/// Enables Readonly attribute as specifed class properties for edit in ProperGrig!
			/// WARNING! Readonly changes for not only THIS class instance, but for all classes with use this class defenition in app domain!
			/// bc. you must to reset this attribute value, imediately after property grid is closed, for avoid unpredictable effects...
			/// 
			/// readonly attribute has all fields in class, not only direct specifed in code!
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void PropertyGrid_SetClassPropertiesReadOnly<T>(
				this T o,
				bool readOnly,
				Func<PropertyDescriptor, bool>? wherePredicate = null)
			{
				PropertyDescriptor[] aProps = TypeDescriptor.GetProperties(o!.GetType()).Cast<PropertyDescriptor>().ToArray();
				if (wherePredicate != null) aProps = aProps.Where(pd => wherePredicate(pd)).ToArray();

				aProps.e_ForEach(pd => pd.e_SetAttribute_ReadOnly(readOnly));
			}

		}





		//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static partial class Extensions_Security_Random
		{

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ComputeHash(this byte[] ab, Secur.HashNames ha)
			{
				using (var H = uom.Secur.CreateHashAlgorithm(ha))
					return H!.ComputeHash(ab);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ComputeHashUni(this string str, Secur.HashNames ha) => str.e_GetBytes_Unicode().e_ComputeHash(ha);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ComputeKeyedHash(this byte[] ab, Secur.KeyedHashNames kha)
			{
				using (var H = uom.Secur.CreateKeyedHashAlgorithm(kha))
					return H!.ComputeHash(ab);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_ComputeKeyedHashUni(this string str, Secur.KeyedHashNames kha) => str.e_GetBytes_Unicode().e_ComputeKeyedHash(kha);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_GetRandomBytes(this int count)
			{
				var bytes = new byte[count];
				if (count > 0)
				{
					var R = new Random();
					R.NextBytes(bytes);
				}
				return bytes;
			}

			// <DebuggerNonUserCode, DebuggerStepThrough>
			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Sub  e_FillRandomBytes(ByRef A() As Byte)
			// Dim R = New Random()
			// Call R.NextBytes(A)
			// End Sub


		}




		//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static partial class Extensions_Structures_Ptr
		{




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static byte[] e_PtrToBytes(this IntPtr lpBuffer, int nBytes)
			{
				var abData = new byte[nBytes];
				Marshal.Copy(lpBuffer, abData, 0, nBytes);
				return abData;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T e_ToStructure<T>(this IntPtr Ptr) where T : struct
			{
				return Marshal.PtrToStructure<T>(Ptr);
				// Dim TTarget = GetType(T)
				// Dim Obj = Marshal.PtrToStructure(Ptr, TTarget)
				// Dim StructFromPtr = CType(Obj, T)
				// Return StructFromPtr
			}

			/// <summary>Последовательно читаем с указателя в массив одинаковых структур</summary>
			/// <param name="iCount">Количество структур для чтения</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] e_ToStructuresSequentially<T>(this IntPtr Ptr, int iCount, int iOffset = 0) where T : struct
			{
				if (iOffset != 0)
					Ptr += iOffset;
				var lResult = new List<T>(iCount);

				// Размер одного элемента структуры
				int iStructSize = Marshal.SizeOf(typeof(T));
				for (int I = 1, loopTo = iCount; I <= loopTo; I++)
				{
					var rStruct = Ptr.e_ToStructure<T>();
					lResult.Add(rStruct);
					Ptr += iStructSize;
				}

				return lResult.ToArray();
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void e_StructureToPtr<T>(this T rStructure, IntPtr PtrToWrite, bool fDeleteOld = false) where T : struct
			{
				Marshal.StructureToPtr(rStructure, PtrToWrite, fDeleteOld);
			}



		}


		/// <summary>Extensions, because otherwise casing fails on Switch==null</summary>
		internal static class Extensions_Switch
		{
			/*
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Switch e_Case<T>(this Switch s, Action<T> a) where T : class
				=> e_Case(s, o => true, a, false);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Switch e_Case<T>(this Switch s, Action<T> a, bool fallThrough) where T : class
				=> e_Case(s, o => true, a, fallThrough);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Switch e_Case<T>(this Switch s, Func<T, bool> c, Action<T> a) where T : class
				=> e_Case(s, c, a, false);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Switch? e_Case<T>(this Switch s,
				Func<T, bool> c, Action<T> a, bool fallThrough) where T : class
			{
				if (s == null) return null;

				T t = s. .Object as T;
				if (t != null)
				{
					if (c(t))
					{
						a(t);
						return fallThrough ? s : null;
					}
				}

				return s;
			}

			

			   Usage:

			 new Switch(foo)
				 .e_Case<Fizz>
					 (action => { doingSomething = FirstMethodCall(); })
				 .e_Case<Buzz>
					 (action => { return false; })
			 
			 */

		}

		/// <summary>Console Extensions</summary>
		internal static class Con
		{

			internal static EventArgs _ConsoleLock = new();


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_WriteConsole(
				this string sText,
				ConsoleColor? clrFore = null,
				ConsoleColor? clrBack = null,
				bool WtiteLine = true,
				StreamWriter? swLog = null)
			{
				lock (_ConsoleLock)
				{
					var clrCurrrenFore = Console.ForegroundColor;
					var clrCurrrentBk = Console.ForegroundColor;
					try
					{
						if (clrFore.HasValue) Console.ForegroundColor = clrFore.Value;
						if (clrBack.HasValue) Console.BackgroundColor = clrBack.Value;

						if (WtiteLine)
						{
							Console.WriteLine(sText);
							swLog?.WriteLine(sText);
						}
						else
						{
							Console.Write(sText);
							swLog?.Write(sText);
						}
					}
					finally
					{
						//Restoring Old Colors
						//if (clrBack.HasValue) Console.BackgroundColor = clrCurrrentBk;
						//if (clrFore.HasValue) Console.ForegroundColor = clrCurrrenFore;
						Console.ResetColor();
					}
				}
			}

			/// <summary>Run Action in Try/catch block And outputs Error to console if occurs</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runTryCatchCon(this Action A, string ActionTitle = "")
			{
				if (!String.IsNullOrWhiteSpace(ActionTitle)) lock (_ConsoleLock) Console.WriteLine($"{ActionTitle}".Trim());
				try { A.Invoke(); }
				catch (Exception ex)
				{
					$"ERROR: {ex.Message}".Trim().e_WriteConsole(ConsoleColor.Yellow, ConsoleColor.DarkRed, false); Console.WriteLine();
				}
			}




			/// <summary>Display progress bar</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_WriteConsoleProgress(
				this float fProgress,
				int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:")
			{
				fProgress = fProgress.e_CheckRange(0, 1);

				var iBarFill = (Int32)((float)ProgressBarLenght * fProgress);
				if (iBarFill > ProgressBarLenght) iBarFill = ProgressBarLenght;

				var sProgressBar = new string(cProgressBarFillChar, iBarFill);
				if (iBarFill < ProgressBarLenght) sProgressBar = sProgressBar.PadRight(ProgressBarLenght, cProgressBarEmptyChar);
				lock (_ConsoleLock)
					Console.Write($"{ProgressPrefixString} [{sProgressBar}] {fProgress.e_FormatPercent(iDecimalPlaces)}\r");
			}

			/// <summary>Display progress bar</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void e_WriteConsoleProgress(
				this int iProgress,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:") =>
				((float)((float)iProgress / (float)100)).e_WriteConsoleProgress(0, ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString);







			/// <summary>Возвращает строку вида 'HEADER---------'</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string e_Console_CreateHeaderLine(this string Text, int iWith = constants.C_DEFAULT_CONSOLE_WIDTH_1)
				=> Text.PadRight(iWith, '-');

			/// <summary>Если строка превышает заданную длинну, то разбивает на строки этой длинны.</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string[] e_Console_SplitStringToFixedWidth(this string sText, int MaxRowWidth = constants.C_DEFAULT_CONSOLE_WIDTH_1)
			{
				var aList = new List<string>();
				while (sText.Length > MaxRowWidth)
				{
					string sLeftPart = sText.Substring(0, MaxRowWidth);
					aList.Add(sLeftPart);
					sText = sText.Substring(MaxRowWidth);
				}

				aList.Add(sText);
				return aList.ToArray();
			}





			/// <summary>Read a password from the console into a SecureString</summary>
			/// <returns>Password stored in a secure string</returns>
			public static SecureString ReadPassword(string Question = constants.CS_ENTER_PWD_EN, char PwdChar = '*')
			{
				lock (_ConsoleLock)
				{
					var password = new SecureString();
					Console.WriteLine(Question);

					// get the first character of the password
					var nextKey = Console.ReadKey(true);
					while (nextKey.Key != ConsoleKey.Enter)
					{
						if (nextKey.Key == ConsoleKey.Backspace)
						{
							if (password.Length > 0)
							{
								password.RemoveAt(password.Length - 1);

								// erase the last * as well
								Console.Write(nextKey.KeyChar);
								Console.Write(" ");
								Console.Write(nextKey.KeyChar);
							}
						}
						else
						{
							password.AppendChar(nextKey.KeyChar);
							Console.Write(PwdChar);
						}

						nextKey = Console.ReadKey(true);
					}

					Console.WriteLine();

					// lock the password down
					password.MakeReadOnly();
					return password;
				}
			}



			internal static string CreateHSplitter(int iWidth = constants.C_DEFAULT_CONSOLE_WIDTH_1) => new('=', iWidth);



			internal static string CreateArgsText(IEnumerable<(string Key, string Value)> ArgsAndDescriptions, int MaxWidth = constants.C_DEFAULT_CONSOLE_WIDTH)
			{
				if (null == ArgsAndDescriptions || !ArgsAndDescriptions.Any()) throw new ArgumentException(nameof(ArgsAndDescriptions));

				var sb = new StringBuilder();
				int iMaxKeyLenght = (from T in ArgsAndDescriptions select T.Key.Length).Max();

				iMaxKeyLenght += 1;
				var aTabbed = ArgsAndDescriptions
					.Select(T => new { Key = (T.Key.PadRight(iMaxKeyLenght) + "-"), Value = (T.Value) });

				aTabbed.e_ForEach(T =>
				{
					sb.Append(T.Key);
					string sDescr = T.Value;
					int iMaxDescrLenght = MaxWidth - iMaxKeyLenght - 2;
					var aLines = sDescr.e_Console_SplitStringToFixedWidth(iMaxDescrLenght);
					if (aLines.Any())
					{
						sb.AppendLine(aLines.First());
						if (aLines.Length > 1)
						{
							var aRows = aLines.Except(aLines.Take(1));
							aRows.e_ForEach(S =>
							{
								string sRow = new string(' ', iMaxKeyLenght + 1) + S;
								sb.AppendLine(sRow);
							});
						}
					}
				});
				return sb.ToString();
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool Console_AskInputIsYes(this string sMessage, string Suffix = " (y/n)?: ", string YesAnswer = "y")
			{
				lock (_ConsoleLock)
				{
					Console.Write(sMessage + Suffix);
					var sInput = Console.ReadLine();
					return (sInput.e_IsNOTNullOrWhiteSpace() && (sInput!.ToLower() == YesAnswer.ToLower()));
				}
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool Console_AskKeyIsYes(this string sMessage)
			{
				lock (_ConsoleLock)
				{
					sMessage += " (y/n)?: ";
					Console.Write(sMessage);
					var kKey = Console.ReadKey();
					Console.WriteLine("");
					if (kKey.Key == ConsoleKey.Y)
						return true;
				}

				return false;
			}

			public static bool Console_IsKeyPressed(ConsoleKey KK = ConsoleKey.Y)
			{
				lock (_ConsoleLock)
				{
					if (Console.KeyAvailable)
					{
						var KKey = Console.ReadKey();
						if (KKey.Key == KK)
							return true;
					}
				}

				return false;
			}


		}


	}

}

#pragma warning restore IDE1006 // Naming Styles
