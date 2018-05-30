// C:\Users\Satyr\Documents\Visual Studio 2015\Projects\Win32 - uczelnia\2.1\Zad5\Zad5\bin\Release\Zad5.exe

.assembly extern mscorlib
{
	.publickeytoken = (
		b7 7a 5c 56 19 34 e0 89
	)
	.ver 4:0:0:0
}
.assembly Zad5
{
	.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = (
		01 00 08 00 00 00 00 00
	)
	.custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = (
		01 00 01 00 54 02 16 57 72 61 70 4e 6f 6e 45 78
		63 65 70 74 69 6f 6e 54 68 72 6f 77 73 01
	)
	.custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = (
		01 00 02 00 00 00 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = (
		01 00 04 5a 61 64 35 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = (
		01 00 00 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = (
		01 00 00 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = (
		01 00 00 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = (
		01 00 04 5a 61 64 35 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = (
		01 00 12 43 6f 70 79 72 69 67 68 74 20 c2 a9 20
		20 32 30 31 37 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = (
		01 00 00 00 00
	)
	.custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = (
		01 00 00 00 00
	)
	.custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = (
		01 00 24 62 30 64 32 34 65 39 32 2d 35 32 33 66
		2d 34 65 62 30 2d 38 64 37 37 2d 32 34 61 66 37
		61 36 61 32 35 35 66 00 00
	)
	.custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = (
		01 00 07 31 2e 30 2e 30 2e 30 00 00
	)
	.custom instance void [mscorlib]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = (
		01 00 1c 2e 4e 45 54 46 72 61 6d 65 77 6f 72 6b
		2c 56 65 72 73 69 6f 6e 3d 76 34 2e 35 2e 32 01
		00 54 0e 14 46 72 61 6d 65 77 6f 72 6b 44 69 73
		70 6c 61 79 4e 61 6d 65 14 2e 4e 45 54 20 46 72
		61 6d 65 77 6f 72 6b 20 34 2e 35 2e 32
	)
	.hash algorithm 0x00008004 // SHA1
	.ver 1:0:0:0
}

.module Zad5.exe
// MVID: {26196293-4D18-45F4-8A5E-7E3FD651D5CD}
.corflags 0x00020003 // ILOnly, Required32Bit, Preferred32Bit


.class private auto ansi '<Module>'
{
} // end of class <Module>

.class public auto ansi sealed Zad5.IncreaseAge
	extends [mscorlib]System.MulticastDelegate
{
	// Methods
	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			object 'object',
			native int 'method'
		) runtime managed 
	{
	} // end of method IncreaseAge::.ctor

	.method public hidebysig newslot virtual 
		instance void Invoke () runtime managed 
	{
	} // end of method IncreaseAge::Invoke

	.method public hidebysig newslot virtual 
		instance class [mscorlib]System.IAsyncResult BeginInvoke (
			class [mscorlib]System.AsyncCallback callback,
			object 'object'
		) runtime managed 
	{
	} // end of method IncreaseAge::BeginInvoke

	.method public hidebysig newslot virtual 
		instance void EndInvoke (
			class [mscorlib]System.IAsyncResult result
		) runtime managed 
	{
	} // end of method IncreaseAge::EndInvoke

} // end of class Zad5.IncreaseAge

.class public auto ansi beforefieldinit Zad5.Program
	extends [mscorlib]System.Object
{
	// Methods
	.method private hidebysig static 
		void Main (
			string[] args
		) cil managed 
	{
		// Method begins at RVA 0x2050
		// Code size 153 (0x99)
		.maxstack 4
		.entrypoint
		.locals init (
			[0] class Zad5.Person adam
		)

		IL_0000: ldstr "Adam"
		IL_0005: ldc.i4.s 19
		IL_0007: ldstr "Polska"
		IL_000c: newobj instance void Zad5.Person::.ctor(string, int32, string)
		IL_0011: stloc.0
		IL_0012: ldstr "Jann"
		IL_0017: ldc.i4.s 55
		IL_0019: ldstr "Polska"
		IL_001e: newobj instance void Zad5.Person::.ctor(string, int32, string)
		IL_0023: dup
		IL_0024: ldstr "Jan"
		IL_0029: callvirt instance void Zad5.Person::set_Name(string)
		IL_002e: ldloc.0
		IL_002f: ldc.i4.s 18
		IL_0031: callvirt instance bool Zad5.Person::IsOlder(int32)
		IL_0036: call void [mscorlib]System.Console::WriteLine(bool)
		IL_003b: callvirt instance string Zad5.Person::GetAge()
		IL_0040: call void [mscorlib]System.Console::WriteLine(string)
		IL_0045: ldloc.0
		IL_0046: ldftn instance void Zad5.Person::IncreaseAge()
		IL_004c: newobj instance void Zad5.IncreaseAge::.ctor(object, native int)
		IL_0051: call void Zad5.Year::add_ev(class Zad5.IncreaseAge)
		IL_0056: ldloc.0
		IL_0057: ldfld int32 Zad5.Person::age
		IL_005c: call void [mscorlib]System.Console::WriteLine(int32)
		IL_0061: call void Zad5.Year::NextYear()
		IL_0066: ldloc.0
		IL_0067: ldfld int32 Zad5.Person::age
		IL_006c: call void [mscorlib]System.Console::WriteLine(int32)
		IL_0071: ldc.i4.s 10
		IL_0073: ldc.i4.s 10
		IL_0075: newobj instance void Zad5.Grid::.ctor(int32, int32)
		IL_007a: dup
		IL_007b: ldc.i4.3
		IL_007c: callvirt instance void Zad5.Grid::DrawRow(int32)
		IL_0081: dup
		IL_0082: ldc.i4.3
		IL_0083: callvirt instance int32[] Zad5.Grid::get_Item(int32)
		IL_0088: ldc.i4.6
		IL_0089: ldc.i4.s 100
		IL_008b: stelem.i4
		IL_008c: ldc.i4.3
		IL_008d: callvirt instance void Zad5.Grid::DrawRow(int32)
		IL_0092: call string [mscorlib]System.Console::ReadLine()
		IL_0097: pop
		IL_0098: ret
	} // end of method Program::Main

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x20f5
		// Code size 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ret
	} // end of method Program::.ctor

} // end of class Zad5.Program

.class public auto ansi beforefieldinit Zad5.Person
	extends [mscorlib]System.Object
{
	// Fields
	.field public int32 age
	.field public string name
	.field public string location

	// Methods
	.method public hidebysig specialname 
		instance string get_Name () cil managed 
	{
		// Method begins at RVA 0x20fd
		// Code size 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld string Zad5.Person::name
		IL_0006: ret
	} // end of method Person::get_Name

	.method public hidebysig specialname 
		instance void set_Name (
			string 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2105
		// Code size 8 (0x8)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldarg.1
		IL_0002: stfld string Zad5.Person::name
		IL_0007: ret
	} // end of method Person::set_Name

	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			string n,
			int32 a,
			string l
		) cil managed 
	{
		// Method begins at RVA 0x210e
		// Code size 28 (0x1c)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ldarg.0
		IL_0007: ldarg.2
		IL_0008: stfld int32 Zad5.Person::age
		IL_000d: ldarg.0
		IL_000e: ldarg.1
		IL_000f: stfld string Zad5.Person::name
		IL_0014: ldarg.0
		IL_0015: ldarg.3
		IL_0016: stfld string Zad5.Person::location
		IL_001b: ret
	} // end of method Person::.ctor

	.method public hidebysig 
		instance void IncreaseAge () cil managed 
	{
		// Method begins at RVA 0x212b
		// Code size 15 (0xf)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldarg.0
		IL_0002: ldfld int32 Zad5.Person::age
		IL_0007: ldc.i4.1
		IL_0008: add
		IL_0009: stfld int32 Zad5.Person::age
		IL_000e: ret
	} // end of method Person::IncreaseAge

	.method public hidebysig 
		instance bool IsOlder (
			int32 a
		) cil managed 
	{
		// Method begins at RVA 0x213b
		// Code size 13 (0xd)
		.maxstack 8

		IL_0000: ldarg.1
		IL_0001: ldarg.0
		IL_0002: ldfld int32 Zad5.Person::age
		IL_0007: ble.s IL_000b

		IL_0009: ldc.i4.1
		IL_000a: ret

		IL_000b: ldc.i4.0
		IL_000c: ret
	} // end of method Person::IsOlder

	.method public hidebysig 
		instance string GetAge () cil managed 
	{
		// Method begins at RVA 0x214c
		// Code size 37 (0x25)
		.maxstack 2
		.locals init (
			[0] int32
		)

		IL_0000: ldarg.0
		IL_0001: ldfld int32 Zad5.Person::age
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: ldc.i4.s 10
		IL_000a: beq.s IL_0013

		IL_000c: ldloc.0
		IL_000d: ldc.i4.s 20
		IL_000f: beq.s IL_0019

		IL_0011: br.s IL_001f

		IL_0013: ldstr "ten"
		IL_0018: ret

		IL_0019: ldstr "twenty"
		IL_001e: ret

		IL_001f: ldstr "other"
		IL_0024: ret
	} // end of method Person::GetAge

	// Properties
	.property instance string Name()
	{
		.get instance string Zad5.Person::get_Name()
		.set instance void Zad5.Person::set_Name(string)
	}

} // end of class Zad5.Person

.class public auto ansi beforefieldinit Zad5.Year
	extends [mscorlib]System.Object
{
	// Fields
	.field private static class Zad5.IncreaseAge ev
	.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
		01 00 00 00
	)
	.field public static int32 year

	// Methods
	.method public hidebysig specialname static 
		void add_ev (
			class Zad5.IncreaseAge 'value'
		) cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x2180
		// Code size 39 (0x27)
		.maxstack 3
		.locals init (
			[0] class Zad5.IncreaseAge,
			[1] class Zad5.IncreaseAge,
			[2] class Zad5.IncreaseAge
		)

		IL_0000: ldsfld class Zad5.IncreaseAge Zad5.Year::ev
		IL_0005: stloc.0
		// loop start (head: IL_0006)
			IL_0006: ldloc.0
			IL_0007: stloc.1
			IL_0008: ldloc.1
			IL_0009: ldarg.0
			IL_000a: call class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
			IL_000f: castclass Zad5.IncreaseAge
			IL_0014: stloc.2
			IL_0015: ldsflda class Zad5.IncreaseAge Zad5.Year::ev
			IL_001a: ldloc.2
			IL_001b: ldloc.1
			IL_001c: call !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class Zad5.IncreaseAge>(!!0&, !!0, !!0)
			IL_0021: stloc.0
			IL_0022: ldloc.0
			IL_0023: ldloc.1
			IL_0024: bne.un.s IL_0006
		// end loop
		IL_0026: ret
	} // end of method Year::add_ev

	.method public hidebysig specialname static 
		void remove_ev (
			class Zad5.IncreaseAge 'value'
		) cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x21b4
		// Code size 39 (0x27)
		.maxstack 3
		.locals init (
			[0] class Zad5.IncreaseAge,
			[1] class Zad5.IncreaseAge,
			[2] class Zad5.IncreaseAge
		)

		IL_0000: ldsfld class Zad5.IncreaseAge Zad5.Year::ev
		IL_0005: stloc.0
		// loop start (head: IL_0006)
			IL_0006: ldloc.0
			IL_0007: stloc.1
			IL_0008: ldloc.1
			IL_0009: ldarg.0
			IL_000a: call class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
			IL_000f: castclass Zad5.IncreaseAge
			IL_0014: stloc.2
			IL_0015: ldsflda class Zad5.IncreaseAge Zad5.Year::ev
			IL_001a: ldloc.2
			IL_001b: ldloc.1
			IL_001c: call !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class Zad5.IncreaseAge>(!!0&, !!0, !!0)
			IL_0021: stloc.0
			IL_0022: ldloc.0
			IL_0023: ldloc.1
			IL_0024: bne.un.s IL_0006
		// end loop
		IL_0026: ret
	} // end of method Year::remove_ev

	.method public hidebysig static 
		void NextYear () cil managed 
	{
		// Method begins at RVA 0x21e7
		// Code size 23 (0x17)
		.maxstack 8

		IL_0000: ldsfld int32 Zad5.Year::year
		IL_0005: ldc.i4.1
		IL_0006: add
		IL_0007: stsfld int32 Zad5.Year::year
		IL_000c: ldsfld class Zad5.IncreaseAge Zad5.Year::ev
		IL_0011: callvirt instance void Zad5.IncreaseAge::Invoke()
		IL_0016: ret
	} // end of method Year::NextYear

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x20f5
		// Code size 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ret
	} // end of method Year::.ctor

	.method private hidebysig specialname rtspecialname static 
		void .cctor () cil managed 
	{
		// Method begins at RVA 0x21ff
		// Code size 11 (0xb)
		.maxstack 8

		IL_0000: ldc.i4 2000
		IL_0005: stsfld int32 Zad5.Year::year
		IL_000a: ret
	} // end of method Year::.cctor

	// Events
	.event Zad5.IncreaseAge ev
	{
		.addon void Zad5.Year::add_ev(class Zad5.IncreaseAge)
		.removeon void Zad5.Year::remove_ev(class Zad5.IncreaseAge)
	}


} // end of class Zad5.Year

.class public auto ansi beforefieldinit Zad5.Grid
	extends [mscorlib]System.Object
{
	.custom instance void [mscorlib]System.Reflection.DefaultMemberAttribute::.ctor(string) = (
		01 00 04 49 74 65 6d 00 00
	)
	// Fields
	.field private int32 rows
	.field private int32 cols
	.field private int32[][] tab

	// Methods
	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			int32 r,
			int32 c
		) cil managed 
	{
		// Method begins at RVA 0x220b
		// Code size 27 (0x1b)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ldarg.0
		IL_0007: ldarg.1
		IL_0008: stfld int32 Zad5.Grid::rows
		IL_000d: ldarg.0
		IL_000e: ldarg.2
		IL_000f: stfld int32 Zad5.Grid::cols
		IL_0014: ldarg.0
		IL_0015: call instance void Zad5.Grid::initTable()
		IL_001a: ret
	} // end of method Grid::.ctor

	.method private hidebysig 
		instance void initTable () cil managed 
	{
		// Method begins at RVA 0x2228
		// Code size 96 (0x60)
		.maxstack 5
		.locals init (
			[0] class [mscorlib]System.Random rnd,
			[1] int32 i,
			[2] int32 k
		)

		IL_0000: ldarg.0
		IL_0001: ldarg.0
		IL_0002: ldfld int32 Zad5.Grid::rows
		IL_0007: newarr int32[]
		IL_000c: stfld int32[][] Zad5.Grid::tab
		IL_0011: newobj instance void [mscorlib]System.Random::.ctor()
		IL_0016: stloc.0
		IL_0017: ldc.i4.0
		IL_0018: stloc.1
		IL_0019: br.s IL_0056
		// loop start (head: IL_0056)
			IL_001b: ldarg.0
			IL_001c: ldfld int32[][] Zad5.Grid::tab
			IL_0021: ldloc.1
			IL_0022: ldarg.0
			IL_0023: ldfld int32 Zad5.Grid::cols
			IL_0028: newarr [mscorlib]System.Int32
			IL_002d: stelem.ref
			IL_002e: ldc.i4.0
			IL_002f: stloc.2
			IL_0030: br.s IL_0049
			// loop start (head: IL_0049)
				IL_0032: ldarg.0
				IL_0033: ldfld int32[][] Zad5.Grid::tab
				IL_0038: ldloc.1
				IL_0039: ldelem.ref
				IL_003a: ldloc.2
				IL_003b: ldloc.0
				IL_003c: ldc.i4.0
				IL_003d: ldc.i4.s 100
				IL_003f: callvirt instance int32 [mscorlib]System.Random::Next(int32, int32)
				IL_0044: stelem.i4
				IL_0045: ldloc.2
				IL_0046: ldc.i4.1
				IL_0047: add
				IL_0048: stloc.2

				IL_0049: ldloc.2
				IL_004a: ldarg.0
				IL_004b: ldfld int32 Zad5.Grid::cols
				IL_0050: blt.s IL_0032
			// end loop

			IL_0052: ldloc.1
			IL_0053: ldc.i4.1
			IL_0054: add
			IL_0055: stloc.1

			IL_0056: ldloc.1
			IL_0057: ldarg.0
			IL_0058: ldfld int32 Zad5.Grid::rows
			IL_005d: blt.s IL_001b
		// end loop

		IL_005f: ret
	} // end of method Grid::initTable

	.method public hidebysig specialname 
		instance int32[] get_Item (
			int32 i
		) cil managed 
	{
		// Method begins at RVA 0x2294
		// Code size 9 (0x9)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld int32[][] Zad5.Grid::tab
		IL_0006: ldarg.1
		IL_0007: ldelem.ref
		IL_0008: ret
	} // end of method Grid::get_Item

	.method public hidebysig specialname 
		instance int32 get_Item (
			int32 i,
			int32 j
		) cil managed 
	{
		// Method begins at RVA 0x229e
		// Code size 11 (0xb)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld int32[][] Zad5.Grid::tab
		IL_0006: ldarg.1
		IL_0007: ldelem.ref
		IL_0008: ldarg.2
		IL_0009: ldelem.i4
		IL_000a: ret
	} // end of method Grid::get_Item

	.method public hidebysig specialname 
		instance void set_Item (
			int32 i,
			int32 j,
			int32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x22aa
		// Code size 12 (0xc)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld int32[][] Zad5.Grid::tab
		IL_0006: ldarg.1
		IL_0007: ldelem.ref
		IL_0008: ldarg.2
		IL_0009: ldarg.3
		IL_000a: stelem.i4
		IL_000b: ret
	} // end of method Grid::set_Item

	.method public hidebysig 
		instance void DrawRow (
			int32 i
		) cil managed 
	{
		// Method begins at RVA 0x22b8
		// Code size 62 (0x3e)
		.maxstack 2
		.locals init (
			[0] int32 k
		)

		IL_0000: ldc.i4.0
		IL_0001: stloc.0
		IL_0002: br.s IL_002a
		// loop start (head: IL_002a)
			IL_0004: ldarg.0
			IL_0005: ldfld int32[][] Zad5.Grid::tab
			IL_000a: ldarg.1
			IL_000b: ldelem.ref
			IL_000c: ldloc.0
			IL_000d: ldelema [mscorlib]System.Int32
			IL_0012: call instance string [mscorlib]System.Int32::ToString()
			IL_0017: ldstr " "
			IL_001c: call string [mscorlib]System.String::Concat(string, string)
			IL_0021: call void [mscorlib]System.Console::Write(string)
			IL_0026: ldloc.0
			IL_0027: ldc.i4.1
			IL_0028: add
			IL_0029: stloc.0

			IL_002a: ldloc.0
			IL_002b: ldarg.0
			IL_002c: ldfld int32 Zad5.Grid::cols
			IL_0031: blt.s IL_0004
		// end loop

		IL_0033: ldstr "\n"
		IL_0038: call void [mscorlib]System.Console::Write(string)
		IL_003d: ret
	} // end of method Grid::DrawRow

	// Properties
	.property instance int32[] Item(
		int32 i
	)
	{
		.get instance int32[] Zad5.Grid::get_Item(int32)
	}
	.property instance int32 Item(
		int32 i,
		int32 j
	)
	{
		.get instance int32 Zad5.Grid::get_Item(int32, int32)
		.set instance void Zad5.Grid::set_Item(int32, int32, int32)
	}

} // end of class Zad5.Grid

