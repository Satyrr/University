%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%       Marcin Gruza - nr indeksu 281050
%
% Wersja języka:
%
% nierekurencyjne procedury bez parametrów z lokalnymi zmiennymi 
% możliwość rekurencyjnego wywoływania procedur 
% możliwość przekazywania parametrów do procedur przez wartość
% możliwość przekazywania parametrów do procedur przez nazwę
%(bez przypiswania parametru call by name do parametru call by name (call_by_name := call_by_name) )
%
%
%
%
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


%%%%%%%%%%%%%%%%%%%%%%  Lexer(rozbudowany z while_parser)  %%%%%%%%%%%%%%%%%%%%%%

lexer(Tokens) -->
   white_space,
   (  (  ":=",      !, { Token = tokAssgn }
      ;  ";",       !, { Token = tokSColon }
      ;  ",",       !, { Token = tokComma }
      ;  "(",       !, { Token = tokLParen }
      ;  ")",       !, { Token = tokRParen }
      ;  "+",       !, { Token = tokPlus }
      ;  "-",       !, { Token = tokMinus }
      ;  "*",       !, { Token = tokTimes }
      ;  "=",       !, { Token = tokEq }
      ;  "<>",      !, { Token = tokNeq }
      ;  "<=",      !, { Token = tokLeq }
      ;  "<",       !, { Token = tokLt }
      ;  ">=",      !, { Token = tokGeq }
      ;  ">",       !, { Token = tokGt }
      ;  digit(D),  !,
            number(D, N),
            { Token = tokNumber(N) }
      ;  letter(L), !, identifier(L, Id),
            {  member((Id, Token), [ (and, tokAnd),
                                     (begin, tokBegin),
                                     (call, tokCall),
                                     (div, tokDiv),
                                     (do, tokDo),
                                     (done, tokDone),
                                     (else, tokElse),
                                     (end, tokEnd),
                                     (fi, tokFi),
                                     (if, tokIf),
                                     (local, tokLocal),
                                     (mod, tokMod),
                                     (not, tokNot),
                                     (or, tokOr),
                                     (procedure, tokProcedure),
                                     (program, tokProgram),
                                     (read, tokRead),
                                     (return, tokReturn),
                                     (then, tokThen),
                                     (value, tokValue),
                                     (while, tokWhile),
                                     (write, tokWrite)]),
               !
            ;  Token = tokId(Id)
            }
      ;  [_],
            { Token = tokUnknown }
      ),
      !,
         { Tokens = [Token | TokList] },
      lexer(TokList)
   ;  [],
         { Tokens = [] }
   ).

white_space -->
   [Char], { code_type(Char, space) }, !,
   white_space.
white_space -->
    [40,42], !,
    comment.
white_space -->
   [].

comment -->
    [42,41],!,
    white_space.
comment -->
    [_],!,
    comment.

digit(D) -->
   [D],
      { code_type(D, digit) }.

digits([D|T]) -->
   digit(D),
   !,
   digits(T).
digits([]) -->
   [].

number(D, N) -->
   digits(Ds),
      { number_chars(N, [D|Ds]) }.

letter(L) -->
   [L], { code_type(L, alpha) }.

alphanum([A|T]) -->
   [A], ({ code_type(A, alnum)  ; A=95 ; A=39 }), !, alphanum(T).
alphanum([]) -->
   [].

identifier(L, Id) -->
   alphanum(As),
      { atom_codes(Id, [L|As]) }.

x(0) --> "".
x(N+1) --> "a", x(N), "b".


max(X,Y,Y):-
  Y > X,
  !.

max(X,_,X).

%%%%%%%%%%%%%%%%%%%%%%  Parser(rozbudowany z while_parser)  %%%%%%%%%%%%%%%%%%%%%%


:- op(990, xfy, ';;').
:- op(900, xfy, :=).
:- op(820, xfy, and).
:- op(840, xfy, or).
:- op(700, xfy, <=).
:- op(700, xfy, <>).

program(program_(Ast)) -->
   [tokProgram],
   [tokId(_)],
   block(Ast).

block(Block) -->
    declarations(Declarations),!,
    [tokBegin],!,
    complexInstruction(CInstr),!,
    [tokEnd],!,
    { Block =.. [block_,Declarations,CInstr] }.
    
%%%%%%%%%%%%%%%%%%%%%%  Parser - Instrukcje  %%%%%%%%%%%%%%%%%%%%%%

complexInstruction(CInstr) -->
    instruction(Instr),
    (	
	    [tokSColon], complexInstruction(Rest), !,
	    { CInstr = [Instr|Rest] }
        ;   
	    [],
	    { CInstr = [Instr|[]] }
    ).

instruction(Instr) -->
    (
	    [tokId(Var), tokAssgn],arithExpr(Expr),!,
	    { Instr = assgn_(Var,Expr) }
        ;
	    [tokIf], boolExpr(Expr), [tokThen], complexInstruction(CInstr1),
	    [tokElse], !, complexInstruction(CInstr2), [tokFi],
	    { Instr = ifelse_(Expr, CInstr1, CInstr2) }
        ;
	    [tokIf],!, boolExpr(Expr), [tokThen], complexInstruction(CInstr),
	    [tokFi], { Instr = if_(Expr, CInstr) }
        ;
	    [tokCall],!, procedureCall(Expr), { Instr = call_(Expr) }
        ;
	    [tokWhile], !, boolExpr(Expr), [tokDo], complexInstruction(CInstr),
	    [tokDone], { Instr = while_(Expr, CInstr) }
        ;
	    [tokReturn], !, arithExpr(Expr), { Instr = return_(Expr) }
        ;
	    [tokRead], !, [tokId(Var)], { Instr = read_(Var) }
        ;
	    [tokWrite], !, arithExpr(Expr), { Instr = write_(Expr) }
    ).
	
%%%%%%%%%%%%%%%%%%%%%%   Deklaracje   %%%%%%%%%%%%%%%%%%%%%%

declarations(Decs) -->
        declaration(Dec),!,
        (   
	    declarations(Rest), { Rest\=void },!,
	    { Decs = [Dec | Rest] }
        ;
	    [],!,
	    { Decs = [Dec|[]] }
        )
    ;
	[],!,
	{ Decs = [] } .

declaration(Dec) -->
	declarator(Declarator),!,
	{ Dec = Declarator }
    ;
	procedure(Proc),!,
	{ Dec = Proc }.

declarator(Dec) -->
    [tokLocal],!,
    variables(Dec).

variables(Vars) -->
    [tokId(Var)],!,
    (
        [tokComma],!,
        variables(Rest),!,
        { Vars = [var(Var) | Rest] }
    ;
        [],!,
        { Vars = [var(Var) | []] }
    ).

procedure(Proc) -->
    [tokProcedure],!,
    [tokId(ProcName)],!,
    formalArguments(Args),!,
    block(Block),!,
    {
        Proc = procedure(ProcName, Args, Block)
    }.

formalArguments(Args) -->
    [tokLParen],!,
    (
	    fArguments(Args),!
        ;
	    [],
	    { Args = [] }
    ),!,
    [tokRParen].

fArguments(Args) -->
    fArgument(Var),!,
    (
	    [tokComma],!,
	    fArguments(Rest),!,
	    { Args = [Var | Rest] }
        ;
	    [],!,
	    {Args = [Var | []]}
    ).

fArgument(Arg) -->
	[tokValue],!,
	[tokId(Var)],!,
	{ Arg = valarg(Var) }
    ;
	[tokId(Var)],!,
	{ Arg = arg(Var) }.

%%%%%%%%%%%%%%%%%%%%%%   Parser - Wyrazenia arytmetyczne   %%%%%%%%%%%%%%%%%%%%%%

arithExpr(NewExpr) -->
    arith2(Expr),!,
    arith1Alt(Expr,NewExpr).

arith1Alt(Expr, NewestExpr) -->
	bin1(Oper),!,
	arith2(Atom),
	{ NewExpr =.. [Oper, Expr, Atom] },!,
	arith1Alt(NewExpr, NewestExpr)
    ;
	[],!,
	{ NewestExpr = Expr }.

arith2(Expr) -->
    (   
	    [tokMinus],
	    arith3(Temp),
	    { Atom = negate(Temp) }
        ;
	    arith3(Atom)
    ),!,
    arith2Alt(Atom,Expr).

arith2Alt(Expr, NewestExpr) -->
    bin2(Oper),!,
    arith3(Atom),
    { NewExpr =.. [Oper, Expr, Atom] },!,
    arith2Alt(NewExpr, NewestExpr)
    ;
    [],!,
    { NewestExpr = Expr }.

arith3(Atom) -->
	procedureCall(Expr),
	{ Atom = Expr}
    ;
	[tokId(Var)],
	{ Atom = var(Var) }
    ;
	[tokNumber(N)],
	{ Atom = const(N) }
    ;
	[tokLParen],arithExpr(Expr), [tokRParen],
	{ Atom = Expr}.

bin1(Oper) -->
	[tokPlus], { Oper = plus } 
    ; 
	[tokMinus], { Oper = minus }.

bin2(Oper) -->
	[tokTimes], { Oper = mult } 
    ;
	[tokDiv], { Oper = divide} 
    ;
	[tokMod], { Oper = modulo }.

procedureCall(Expr) -->
    [tokId(Id)],
    [tokLParen], !,
    realArgs(Args),
    [tokRParen],
    { Expr = function(Id, Args) }.

realArgs(Args) -->
	(
	    arithExpr(Expr),
	    [tokComma],
	    realArgs(ArgsRest),
	    { Args = [Expr | ArgsRest] }
	;
	    arithExpr(Expr),[],
	    { Args = [Expr | []]  }
	)
    ; 
	[], { Args = [] }.

%%%%%%%%%%%%%%%%%%%%%%   Parser - Wyrazenia logiczne   %%%%%%%%%%%%%%%%%%%%%%

boolExpr(NewExpr) -->
    bool2(Expr),!,
    bool1Alt(Expr, NewExpr).

bool1Alt(Expr, NewestExpr) -->
	[tokOr],!,
	bool2(Atom),
	{ NewExpr = or_(Expr, Atom) },!,
	bool1Alt(NewExpr, NewestExpr)
    ;
	[],!,
	{ NewestExpr = Expr }.

bool2(Expr) -->
    (   
	 [tokNot],
        bool3(Temp),
        { Atom = not_(Temp) }
    ;
        bool3(Atom)
    ), !,
    bool2alt(Atom, Expr).

bool2alt(Expr, NewestExpr) -->
  [tokAnd],[tokNot],!,
  bool3(Atom),
  { NewExpr = not_(and_(Expr, Atom))},!,
  bool2alt(NewExpr, NewestExpr)
    ;
	[tokAnd],!,
	bool3(Atom),
	{ NewExpr = and_(Expr, Atom)},!,
	bool2alt(NewExpr, NewestExpr)
    ;
	[],!,
	{ NewestExpr = Expr }.

bool3(Atom) -->
	arithExpr(Expr1),!,
	rel_op(Oper),!,
	arithExpr(Expr2),!,
	{ Atom =..[Oper,Expr1,Expr2] }
    ;
	[tokLParen],boolExpr(Expr),[tokRParen],
	{ Atom = Expr}.

rel_op(eq) -->
   [tokEq], !.
rel_op(neq) -->
   [tokNeq], !.
rel_op(leq) -->
   [tokLeq], !.
rel_op(lt) -->
   [tokLt], !.
rel_op(gt) -->
   [tokGt], !.
rel_op(geq) -->
   [tokGeq].

%%%%%%%%%%%%%%%%%%%%%%%   Kompilacja do macroassemblera   %%%%%%%%%%%%%%%%%%%%%%

toMacroAssem(program_(block_(Declarations, Instructions)), MacroAssembler, Thunks, ThunksCount, LabelCount, MacroAssemblerFuncList):-
    varEnv(Declarations, 1, [], VarEnv, global, var),
    procEnv(Declarations, [], ProcEnv),
    length(VarEnv, LocalVarsCount),
    StackPos is 65535 - LocalVarsCount - 2,  %% poczatkowy adres szczytu stosu
    BPPos is StackPos - 2,	%% adres BP dla maina
    StackInit = [const(StackPos), store(65535), const(label(_,mainreturn_)), pushacc, const(0), pushacc, const(BPPos), store(65534)],
    macroassemblerInstructions(Instructions, VarEnv, ProcEnv, StackInit, MacroAssemWithoutHalt, [], 0, ThunksAcc,  ThunksAccCount, 0, MainLabelCount),
    append(MacroAssemWithoutHalt,[label(_,mainreturn_),const(0),syscall],MacroAssembler), % dolaczenie syscall halt do programu
    asFunctions(VarEnv, ProcEnv, ProcEnv, [], MacroAssemblerFuncList, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, MainLabelCount, LabelCount). % asemblacja funkcji 
    
	
%%%%%%%%%%%%%%%%%%%%%%   Slowniki zmiennych i funkcji   %%%%%%%%%%%%%%%%%%%%%%
	
	
varEnv([[var(VarName) | VarRest] | Rest], Offset, Acc, VarEnv, Scope, Type):-
        !, NewAcc = [(VarName, Offset, Scope, Type) | Acc],
        NewOffset is Offset + 1,
        varEnv([VarRest | Rest], NewOffset, NewAcc, VarEnv, Scope, Type).
varEnv([[var(VarName) | []] | Rest], Offset, Acc, VarEnv, Scope, Type):-
        !,NewAcc = [(VarName, Offset, Scope, Type) | Acc],
        NewOffset is Offset + 1,
        varEnv(Rest, NewOffset, NewAcc, VarEnv, Scope, Type).
varEnv([arg(ArgName) | Rest], Offset, Acc, VarEnv, Scope, Type):-
        !, NewAcc = [(ArgName, Offset, Scope, arg) | Acc],
        NewOffset is Offset - 1,
        varEnv(Rest, NewOffset, NewAcc, VarEnv, Scope, Type).
varEnv([valarg(ArgName) | Rest], Offset, Acc, VarEnv, Scope, Type):-
        !, NewAcc = [(ArgName, Offset, Scope, valarg) | Acc],
        NewOffset is Offset - 1,
        varEnv(Rest, NewOffset, NewAcc, VarEnv, Scope, Type).
varEnv([_ | Rest], Offset, Acc, VarEnv, Scope, Type):-
        !, varEnv(Rest, Offset, Acc, VarEnv, Scope, Type).
varEnv([], _, VarEnv, VarEnv, _, _).


procEnv([procedure(ProcName, ArgList, block_(Declarations, Instructions)) | ProcRest], Acc, ProcEnv):-
	!, NewAcc = [(ProcName, ArgList, Declarations, Instructions) | Acc],
	procEnv(ProcRest, NewAcc, ProcEnv).
procEnv([_ | ProcRest], Acc, ProcEnv):-
	procEnv(ProcRest, Acc, ProcEnv).
procEnv([], ProcEnv, ProcEnv).	 


%%%%%%%%%%%%%%%%%%%%%%   Funkcje   %%%%%%%%%%%%%%%%%%%%%%


asFunctions(_, _, [], MacroAssemFuncList, MacroAssemFuncList, Thunks, ThunksCount, Thunks, ThunksCount, LabelCount, LabelCount).

asFunctions(VarEnv, ProcEnv, [(ProcName, ArgList, Declarations, Instructions) | FunctionsRest], Acc, MacroAssemFuncList, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCountAcc, LabelCount):-
  varEnv(Declarations, 1, VarEnv, FuncVarEnv, local, var),	%zmienne lokalne/globalne
  length(ArgList, ArgOffset),
  varEnv(ArgList, ArgOffset, FuncVarEnv, FuncEnv, local, args),	%argumenty + zmienne lokalne/globalne
  length(VarEnv, GlobalVars),
  length(FuncVarEnv, GlobalAndLocalVars),
  LocalVarsNumber is GlobalVars - GlobalAndLocalVars,
  StackInit = [label(_,function(ProcName)),spcontrol(LocalVarsNumber)],	%rezerwacja miejsca dla zmiennych lokalnych
  macroassemblerInstructions(Instructions, FuncEnv, ProcEnv, StackInit, MacroAssem, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, LabelCountAcc, NewLabelCountAcc),
  append(MacroAssem,[const(0),store(65533),load(65534),store(65535),pop,store(65534),pop,jump],MacroAssemDefaultRet),
  asFunctions(VarEnv, ProcEnv, FunctionsRest, [(ProcName, MacroAssemDefaultRet)|Acc], MacroAssemFuncList, NewThunksAcc, NewThunksAccCount, Thunks, ThunksCount, NewLabelCountAcc, LabelCount).
  
  

%%%%%%%%%%%%%%%%%%%%%%   Kompilacja instrukcji do makroassemblera   %%%%%%%%%%%%%%%%%%%%%%  
  
macroassemblerInstructions([return_(Expr) | []], VarEnv, ProcEnv, Acc, MacroAssem, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, NewLabelCount, NewLabelCount) :-
  !, compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, [], ExprCode),
  concat([Acc, ExprCode, [pop, store(65533),load(65534),store(65535),pop,store(65534),pop,jump]], MacroAssem).
  
macroassemblerInstructions([], _, _, MacroAssem, MacroAssem, Thunks, ThunksCount, Thunks, ThunksCount, NewLabelCount, NewLabelCount) :-!.
       
macroassemblerInstructions([Instr | Instructions], VarEnv, ProcEnv, Acc, MacroAssem, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCount, NewLabelCount) :-
  (
    ( 
	  Instr = assgn_(Var, Expr), %% przypisanie
	  compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, [], ExprCode),
	  findvar(VarEnv, Var, (Var, Offset, Scope, Type)),
	  (  
  		Scope = global, !,
  		VarAddr is 65535 - Offset - 2,
  		AssgnCode = [pop, store(VarAddr)]
  	    ;
  		Type = var,
  		NewOffset is -Offset,
  		AssgnCode = [pop, store((bp, NewOffset))]
  	    ;
  		Type = valarg,
  		NewOffset is Offset + 1, 
  		AssgnCode = [pop, store((bp, NewOffset))]
	  ),
	  concat([Acc,ExprCode, AssgnCode], NewAcc),
	  LabelCountAcc is LabelCount

      ;

	  Instr = ifelse_(BoolExpr, CInstr1, CInstr2), !,  %% if-else
	  compile_boolean(VarEnv, ProcEnv, BoolExpr, ThunksAcc, ThunksAccCount, NewThunksBoolAcc, NewThunksBoolAccCount, LabelCount, BoolLabelCount, [], BoolCode),
	  macroassemblerInstructions(CInstr1, VarEnv, ProcEnv, [], CInstr1Code, NewThunksBoolAcc, NewThunksBoolAccCount, NewThunksCinstr1Acc, NewThunksCinstr1AccCount, BoolLabelCount, CInstr1LabelCount),
	  macroassemblerInstructions(CInstr2, VarEnv, ProcEnv, [], CInstr2Code, NewThunksCinstr1Acc, NewThunksCinstr1AccCount, NewThunksAcc, NewThunksAccCount, CInstr1LabelCount, CInstr2LabelCount),
	  concat([Acc, BoolCode, [pop, branchz(label(CInstr2LabelCount, else))], CInstr1Code, [jump(label(CInstr2LabelCount, end)), label(CInstr2LabelCount, else)], CInstr2Code, [label(CInstr2LabelCount, end)]], NewAcc),
	  LabelCountAcc is CInstr2LabelCount + 1

      ;

	  Instr = if_(BoolExpr, CInstr), !, %% if 
	  compile_boolean(VarEnv, ProcEnv, BoolExpr, ThunksAcc, ThunksAccCount, NewThunksBoolAcc, NewThunksBoolAccCount, LabelCount, BoolLabelCount, [], BoolCode),
	  macroassemblerInstructions(CInstr, VarEnv, ProcEnv, [], CInstrCode, NewThunksBoolAcc, NewThunksBoolAccCount, NewThunksAcc, NewThunksAccCount, BoolLabelCount, CInstrLabelCount),
	  concat([Acc, BoolCode, [pop, branchz(label(CInstrLabelCount, end))], CInstrCode, [label(CInstrLabelCount, end)]], NewAcc),
	  LabelCountAcc is CInstrLabelCount + 1

      ;

	  Instr = call_(Func), !, %% call
	  compileExpr(VarEnv, ProcEnv, Func, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, [], FuncAcc),
	  concat([Acc, FuncAcc, [pop]], NewAcc),
	  LabelCountAcc is LabelCount

      ;

	  Instr = while_(BoolExpr, CInstr), %% while
	  compile_boolean(VarEnv, ProcEnv, BoolExpr, ThunksAcc, ThunksAccCount, NewThunksBoolAcc, NewThunksBoolAccCount, LabelCount, BoolLabelCount, [], BoolCode),
	  macroassemblerInstructions(CInstr, VarEnv, ProcEnv, [], CInstrCode, NewThunksBoolAcc, NewThunksBoolAccCount, NewThunksAcc, NewThunksAccCount, BoolLabelCount, CInstrLabelCount),
	  concat([Acc, [label(CInstrLabelCount, while)], BoolCode, [pop, branchz(label(CInstrLabelCount, end))], CInstrCode, [jump(label(CInstrLabelCount, while)), label(CInstrLabelCount, end)]], NewAcc),
	  LabelCountAcc is CInstrLabelCount + 1

      ;

	  Instr = return_(Expr), !, %% return
	  compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, [], ExprCode),
	  concat([Acc, ExprCode, [pop, store(65533),load(65534),store(65535),pop,store(65534),pop,jump]], NewAcc),
	  LabelCountAcc is LabelCount

      ;

	  Instr = read_(Var),    %% read
	  findvar(VarEnv, Var, (Var, Offset, Scope, Type)),
	  (
  		Scope = global, !,
  		VarAddr is 65535 - Offset - 2,
  		AssgnCode = [store(VarAddr)]
  	    ;
  		Type = var, !,
  		Pos is -Offset, 
  		AssgnCode = [store((bp, Pos))]
  	    ;
  		Type = valarg, !,
  		Pos is Offset + 1,
  		AssgnCode = [store((bp, Pos))]
	  ),
	  concat([Acc, [const(1), syscall], AssgnCode], NewAcc),
	  LabelCountAcc is LabelCount

      ;

	  Instr = write_(Expr), !,  %% write
	  compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, [], ExprCode),
	  concat([Acc, ExprCode, [pop, swapd, const(2), syscall]], NewAcc),
	  LabelCountAcc is LabelCount

     ;

    Instr = read_(Var), !, %% read dla argumentu call by name, wartosc zapisywana na koncu sekcji kodu programu
    findvar(VarEnv, Var, (Var, Offset, _, Type)),
    Type = arg,
    Pos is Offset + 1,
    NewThunksAccCount is ThunksAccCount + 1,
    append(ThunksAcc, [([const(label(NewThunksAccCount,thunkconst(0))), swapa, load, pushacc], NewThunksAccCount, [label(NewThunksAccCount,thunkconst(0))])], NewThunksAcc),
    append(Acc, [const(1), syscall, store(label(NewThunksAccCount,thunkconst(0))), const(label(_,thunk(NewThunksAccCount))), store((bp,Pos))], NewAcc),
    LabelCountAcc is LabelCount         

    ;

    Instr = assgn_(Var, Expr),!, %% przypisanie dla argumentu call by name
    compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, ThunksAcc1, ThunksAccCount1, [], ExprCode),
    findvar(VarEnv, Var, (Var, Offset, _, Type)),
    Type = arg,
    NewOffset is Offset + 1,
    NewThunksAccCount is ThunksAccCount1 + 1,
    thunksVars(ExprCode, NewExprCode, ThunksVarsAddrCode, NewThunksAccCount, 0, ThunkLabels),
    append(ThunksAcc1, [(NewExprCode, NewThunksAccCount, ThunkLabels)], NewThunksAcc),
    concat([Acc, [const(label(_,thunk(NewThunksAccCount))), store((bp, NewOffset))], ThunksVarsAddrCode ], NewAcc),
    LabelCountAcc is LabelCount

    ),
    macroassemblerInstructions(Instructions, VarEnv, ProcEnv, NewAcc, MacroAssem, NewThunksAcc, NewThunksAccCount, Thunks, ThunksCount, LabelCountAcc, NewLabelCount)
  ).

%% kompiluje wyrazenie pozostawiajac wynik na szczycie stosu, Thunks to funkcje pomocnicze dla argumentow call by name
compileExpr(VarEnv, ProcEnv, Expr, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, Acc, List):-
	(
  		Expr = function(Name, Args),!,
  		findproc(ProcEnv, Name, (Name, ArgList ,_ ,_)),
  		compile_arguments(VarEnv, ProcEnv, Args, ArgList, [], ArgumentsPush, 0, ClearStack, ThunksAcc, ThunksAccCount, Thunks, ThunksCount),
  		concat([Acc, ArgumentsPush, [call(Name), ClearStack, load(65533), pushacc]], List)
		;
  		Expr = var(X1), !,
  		findvar(VarEnv, X1, (X1, Offset, Scope, Type)),
  		append(Acc, [pushvar(X1, Offset, Scope, Type)], List),
  		Thunks = ThunksAcc,
  		ThunksCount = ThunksAccCount
		;
  		Expr = const(N), !,
  		append(Acc, [pushconst(N)], List),
  		Thunks = ThunksAcc,
  		ThunksCount = ThunksAccCount
		;
  		Expr = negate(X), !,
  		compileExpr(VarEnv, ProcEnv, X, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, [], List1),
  		concat([Acc, List1, [load((sp, 0)), subaddr((sp, 0)), subaddr((sp, 0)), store((sp, 0))]], List),
  		Thunks = ThunksAcc,
  		ThunksCount = ThunksAccCount
		;
  		Expr = plus(X1, X2), !,
  		compileExpr(VarEnv, ProcEnv, X1, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
  		compileExpr(VarEnv, ProcEnv, X2, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
  		concat([List1, List2, [pop, addaddr((sp, 0)), store((sp, 0))]],List)
		;
  		Expr = minus(X1, X2), !,
  		compileExpr(VarEnv, ProcEnv, X2, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
  		compileExpr(VarEnv, ProcEnv, X1, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
  		concat([List1, List2, [pop, subaddr((sp, 0)), store((sp, 0))]], List)
		;
  		Expr = mult(X1, X2), !,
  		compileExpr(VarEnv, ProcEnv, X1, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
  		compileExpr(VarEnv, ProcEnv, X2, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
  		concat([List1, List2, [pop, multaddr((sp, 0)), store((sp, 0))]], List)
		;
  		Expr = divide(X1, X2), !,
  		compileExpr(VarEnv, ProcEnv, X2, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
  		compileExpr(VarEnv, ProcEnv, X1, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
  		concat([List1, List2, [pop, divaddr((sp, 0)), store((sp, 0))]], List)
		;
  		Expr = modulo(X1, X2), !,
  		compileExpr(VarEnv, ProcEnv, X2, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
  		compileExpr(VarEnv, ProcEnv, X1, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
  		concat([List1, List2, [load((sp,0)), divaddr((sp, 1)), multaddr((sp, 1)), store((sp, 1)), load((sp, 0)), subaddr((sp, 1)), store((sp, 1)), spcontrol(1)]], List)
	).

%% kompiluje wyrazenie typu bool, pozostawia na stosie 0 w przypadku falszu lub dowolna liczbe dodatnia dla prawdy
compile_boolean(VarEnv, ProcEnv, BoolExpr, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCount, NewLabelCount, Acc, List):-
  (
    	BoolExpr = or_(Expr1, Expr2),
    	compile_boolean(VarEnv, ProcEnv, Expr1, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, LabelCount, LabelCount1, [], List1),
    	compile_boolean(VarEnv, ProcEnv, Expr2, NewThunksAcc, NewThunksAccCount, Thunks, ThunksCount, LabelCount1, NewLabelCount, [], List2),
    	concat([Acc, List1, List2, [pop, addaddr((sp, 0)), store((sp, 0))]], List)
    ;
    	BoolExpr = not_(Expr1),
    	compile_boolean(VarEnv, ProcEnv, Expr1, ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCount, LabelCount1, [], List1),
    	concat([Acc, List1, [pop, branchz(label(LabelCount1, zero)), const(0), pushacc, jump(label(LabelCount1, end)), label(LabelCount1, zero), const(1), pushacc, label(LabelCount1,end) ]],List),
    	NewLabelCount is LabelCount1 + 1
    ;
    	BoolExpr = and_(Expr1, Expr2),
    	compile_boolean(VarEnv, ProcEnv, Expr1, ThunksAcc, ThunksAccCount, NewThunksAcc, NewThunksAccCount, LabelCount, LabelCount1, [], List1),
    	compile_boolean(VarEnv, ProcEnv, Expr2, NewThunksAcc, NewThunksAccCount, Thunks, ThunksCount, LabelCount1, NewLabelCount, [], List2),
    	concat([Acc, List1, List2, [pop, multaddr((sp, 0)), store((sp, 0))]], List)
    ;
    	BoolExpr = eq(Expr1, Expr2),
    	compileExpr(VarEnv, ProcEnv, Expr1, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
    	compileExpr(VarEnv, ProcEnv, Expr2, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
    	concat([Acc, List1, List2, [pop,  subaddr((sp, 0)), branchz(label(LabelCount, zero)), const(0), store((sp, 0)), jump(label(LabelCount, end)), label(LabelCount, zero), const(1), store((sp, 0)), label(LabelCount, end) ]], List),
    	NewLabelCount is LabelCount + 1
    ;
    	BoolExpr = neq(Expr1, Expr2),
    	compileExpr(VarEnv, ProcEnv, Expr1, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
    	compileExpr(VarEnv, ProcEnv, Expr2, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
    	concat([Acc, List1, List2, [pop, subaddr((sp, 0)), branchz(label(LabelCount, zero)), const(1), store((sp, 0)), jump(label(LabelCount, end)), label(LabelCount, zero), const(0), store((sp,0)), label(LabelCount,end) ]], List),
    	NewLabelCount is LabelCount + 1
    ;
    	BoolExpr = leq(Expr1, Expr2),
    	compileExpr(VarEnv, ProcEnv, Expr2, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
    	compileExpr(VarEnv, ProcEnv, Expr1, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
    	concat([Acc, List1, List2, [load((sp,0)), branchn(label(LabelCount,expr1_less0)), load((sp,1)), branchn(label(LabelCount,nleq)),jump(label(LabelCount, same_sign)), label(LabelCount,expr1_less0), load((sp,1)), branchn(label(LabelCount,same_sign)), jump(label(LabelCount, leq)), label(LabelCount,same_sign), pop,  subaddr((sp, 0)), branchz(label(LabelCount, leq)), branchn(label(LabelCount, leq)), label(LabelCount,nleq), const(0),store((sp, 0)), jump(label(LabelCount, end)), label(LabelCount, leq), const(1), store((sp, 0)), label(LabelCount, end) ]], List),
    	NewLabelCount is LabelCount + 1
    ;
    	BoolExpr = lt(Expr1, Expr2),
    	compileExpr(VarEnv, ProcEnv, Expr2, ThunksAcc, ThunksAccCount, ThunksNewAcc, TNewAccCount, [], List1),
    	compileExpr(VarEnv, ProcEnv, Expr1, ThunksNewAcc, TNewAccCount, Thunks, ThunksCount, [], List2),
    	concat([Acc, List1, List2, [load((sp,0)), branchn(label(LabelCount,expr1_less0)), load((sp,1)), branchn(label(LabelCount,nlt)),jump(label(LabelCount, same_sign)), label(LabelCount,expr1_less0), load((sp,1)), branchn(label(LabelCount,same_sign)), jump(label(LabelCount, lt)), label(LabelCount,same_sign), pop,  subaddr((sp, 0)), branchn(label(LabelCount, lt)), label(LabelCount,nlt), const(0), store((sp, 0)), jump(label(LabelCount, end)), label(LabelCount, lt), const(1), store((sp, 0)), label(LabelCount, end) ]], List),
    	NewLabelCount is LabelCount + 1
    ;
    	BoolExpr = geq(Expr1, Expr2),
    	compile_boolean(VarEnv, ProcEnv, leq(Expr2, Expr1), ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCount, NewLabelCount, Acc, List)
    ;
    	BoolExpr = gt(Expr1, Expr2),
    	compile_boolean(VarEnv, ProcEnv, lt(Expr2, Expr1), ThunksAcc, ThunksAccCount, Thunks, ThunksCount, LabelCount, NewLabelCount, Acc, List)
  ).

%% kod obslugujacy wrzucenie i usuniecie argumentow funkcji ze stosu
compile_arguments(_, _, [], [], Push, Push, ClearCount, spcontrol(ClearCount), Thunks, ThunksCount, Thunks, ThunksCount):-!.
compile_arguments(VarEnv, ProcEnv, [Argument | Rest], [ArgumentInfo | RestInfo], PushAcc, Push, ClearCount, ClearStack, ThunksAcc, ThunksAccCount, Thunks, ThunksCount):-
  (
    ArgumentInfo = valarg(_), !,
    compileExpr(VarEnv, ProcEnv, Argument, ThunksAcc, ThunksAccCount, ThunksAccNew2, ThunksNewAccCount2, [], ArgumentCode),
    append(PushAcc, ArgumentCode, NewPushAcc)
    ;
    ArgumentInfo = arg(_), !,
    compileExpr(VarEnv, ProcEnv, Argument, ThunksAcc, ThunksAccCount, ThunksAccNew, ThunksNewAccCount, [], ArgumentCode),
    ThunksNewAccCount2 is ThunksNewAccCount + 1,
    thunksVars(ArgumentCode, NewArgumentCode, ThunksVarsAddrCode, ThunksNewAccCount2, 0, ThunkLabels),
    append(ThunksAccNew, [(NewArgumentCode, ThunksNewAccCount2, ThunkLabels)], ThunksAccNew2),
    concat([PushAcc, [const(label(_, thunk(ThunksNewAccCount2))), pushacc], ThunksVarsAddrCode], NewPushAcc)
  ),
  NewClearCount is ClearCount + 1,
  compile_arguments(VarEnv, ProcEnv, Rest, RestInfo, NewPushAcc, Push, NewClearCount, ClearStack, ThunksAccNew2, ThunksNewAccCount2, Thunks, ThunksCount).

% odwolania do zmiennych w funkcjach pomocniczych call by name
thunksVars([],[],[], _, _, []):-!.
thunksVars([pushvar(Name, Offset, local, Type) | Rest], Code, ThunksVarsAddrCode, ThunkNr, VarNr, ThunkLabels):-
  (
    Type = var,!,
    append([pushvar(Name, Offset, local, Type), pop, store(label(ThunkNr,thunkvar(VarNr)))],RestThunksVarsAddrCode, ThunksVarsAddrCode),
    append([const(label(ThunkNr,thunkvar(VarNr))), swapa, load, pushacc], RestCode, Code)
    ;
    Type = valarg,!,
    append([pushvar(Name, Offset, local, Type), pop, store(label(ThunkNr,thunkvar(VarNr)))], RestThunksVarsAddrCode, ThunksVarsAddrCode),
    append([const(label(ThunkNr,thunkvar(VarNr))), swapa, load, pushacc], RestCode, Code)
    ;
    Type = arg,!,
    ArgOffset is Offset + 1,
    append([load((bp,ArgOffset)), store(label(ThunkNr,thunkvar(VarNr)))], RestThunksVarsAddrCode, ThunksVarsAddrCode ),
    append([const(label(ThunkNr,thunkvarret(ThunkNr))), pushacc, const(label(ThunkNr,thunkvar(VarNr))), swapa, load, jump, label(ThunkNr,thunkvarret(ThunkNr)), load(65533), pushacc], RestCode, Code)
  ),
  ThunkLabels = [(label(ThunkNr,thunkvar(VarNr))) | RestThunkLabels],
  NewVarNr is VarNr + 1,
  thunksVars(Rest, RestCode, RestThunksVarsAddrCode, ThunkNr, NewVarNr, RestThunkLabels).
thunksVars([Instr | Rest], [Instr | NewCode], ThunksVarsAddrCode, ThunkNr, VarNr, ThunkLabels):-!,
  thunksVars(Rest, NewCode, ThunksVarsAddrCode, ThunkNr, VarNr, ThunkLabels).

%%%%%%%%%%%%%%%%%%%%%%   Makroassembler -> Sextium assembler   %%%%%%%%%%%%%%%%%%%%%%

translate(SextiumAssembler, LabelCount,Labels) -->
    [pushvar(_, Offset, local, arg)],!,
    {
        NewLabelCount is LabelCount + 1,
        ArgPos is Offset + 1,
        Code = [const(65535), swapa, load, swapd, const(1), swapd, sub, swapa, const(label(NewLabelCount,thunkret)), store, const(65535), swapa, store, const(65534),swapa,load,swapd,const(ArgPos),add,swapa,load, jump, label(NewLabelCount,thunkret), const(65535), swapa, load, swapd, const(1), swapd, sub, swapd, const(65533), swapa, load, swapd, swapa, swapd, store, const(65535), swapa, store]
    },
    translate(SextiumAssemblerRest, NewLabelCount,Labels),
    { append(Code, SextiumAssemblerRest, SextiumAssembler) }.
    

translate(SextiumAssembler, LabelCount,Labels) -->	
    [call(Name)], !,
    {
	     NewLabelCount is LabelCount + 1,
	     ReturnAddrPush = [const(65535),swapa,load,swapd,const(1),swapd,sub,swapa,const(label(NewLabelCount,return)),store],
        BPPush = [const(65534),swapa,swapd,load,swapa,const(1),swapd,sub,swapa,store],
        StackMove = [const(65535),swapa,store,swapa,const(65534),swapa,store],
	FuncJump = [const(label(_,function(Name))),jump,label(NewLabelCount,return)],
        concat([ReturnAddrPush,BPPush,StackMove,FuncJump],Code)
    },
    translate(SextiumAssemblerRest, NewLabelCount,Labels),
    { append(Code, SextiumAssemblerRest, SextiumAssembler) }.
	
translate(SextiumAssembler, LabelCount,Labels) -->
    (
	    [pushvar(_, Offset, global,_)], !,
	    { 
	      Addr is 65535 - Offset - 2,
	      Code = [const(Addr),swapa,load,swapd,const(65535),swapa,load,swapd,swapa,
		    const(1),swapd,sub,swapa,store,const(65535),swapa,store] 
	    }
        ;
	    [pushvar(_, Offset, local, var)], !,
	    {
	      VarPos is -Offset,
	      Code = [const(65534),swapa,load,swapd,const(VarPos),add,swapa,load,swapd,
		  const(65535),swapa,load,swapa,const(1),swapd,swapa,sub,swapa,store,const(65535),swapa,store]
	    }
        ;
	    [pushvar(_, Offset, local, valarg)], !,
	    {
	      ArgPos is Offset + 1,
	      Code = [const(65534),swapa,load,swapd,const(ArgPos),add,swapa,load,swapd ,const(65535),swapa,load,swapa,const(1),swapd,swapa,sub,swapa,store,const(65535),swapa,store]
	    }
        ;
	    [pushconst(Const)], !,
	    {
		Code = [const(65535),swapa,load,swapd,const(1),swapd,sub,swapa,const(Const),store,const(65535),
			swapa,store]
	    }
        ;
	    [pushacc], !,
	    {
		Code = [swapd,const(65535),swapa,load,swapd,swapa,const(1),swapd,sub,swapa,store,const(65535),swapa,store]
	    }
        
        ;
        
	    [spcontrol(Count)], !,
	    {
		Code = [const(Count),swapd,const(65535),swapa,load,add,store]
	    }
	
	;
	
	    [store((sp,Offset))], !,
	    {
		Code = [swapd,const(65535),swapa,load,swapd,swapa,const(Offset),add,swapa,store]
	    }
	;
	    [store((bp,Offset))], !,
	    {
		Code = [swapd,const(65534),swapa,load,swapd,swapa,const(Offset),add,swapa,store]
	    }
	;
	    [store(Adr)], !,
	    {
		Code = [swapa,const(Adr),swapa,store]
	    }
	
	;
	
	    [load((sp,Offset))], !,
	    {
		Code = [const(65535),swapa,load,swapd,const(Offset),add,swapa,load]
	    }
	;
	    [load((bp,Offset))], !,
	    {
		Code = [const(65534),swapa,load,swapd,const(Offset),add,swapa,load]
	    }
	;
	    [load(Adr)], !,
	    {
		Code = [const(Adr),swapa,load]
	    }
	
	;
	
	    [pop], !,
	    {
		Code = [const(65535),swapa,load,swapa,load,swapa,swapd,const(1),add,swapa,swapd,const(65535),swapa,store,swapd]
	    }
	
	;
	
	    [branchz(label(Nr,Name))], !,
	    {
		Code = [swapa,const(label(Nr,Name)),swapa,branchz]
	    }
	;
	    [branchn(label(Nr,Name))], !,
	    {
		Code = [swapa,const(label(Nr,Name)),swapa,branchn]
	    }
	
	;
	
	    [jump(label(Nr,Name))], !,
	    {
		Code = [const(label(Nr,Name)),jump]
	    }
	
	;
	
	    [addaddr((sp,Offset))], !,
	    {
		Code = [swapd, const(65535),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,add]
	    }
	;
	    [addaddr((bp,Offset))], !,
	    {
		Code = [swapd, const(65534),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,add]
	    }
	;
	    [addaddr(Adr)], !,
	    {
		Code = [swapd, const(Adr),swapa,load,swapd,add]
	    }
	
	;
	
	    [subaddr((sp,Offset))], !,
	    {
		Code = [swapd, const(65535),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,sub]
	    }
	;
	    [subaddr((bp,Offset))], !,
	    {
		Code = [swapd, const(65534),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,sub]
	    }
	;
	    [subaddr(Adr)], !,
	    {
		Code = [swapd, const(Adr),swapa,load,swapd,sub]
	    }
	
	;
	
	    [multaddr((sp,Offset))], !,
	    {
		Code = [swapd, const(65535),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,mult]
	    }
	;
	    [multaddr((bp,Offset))], !,
	    {
		Code = [swapd, const(65534),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,mult]
	    }
	;
	    [multaddr(Adr)], !,
	    {
		Code = [swapd, const(Adr),swapa,load,swapd,mult]
	    }
	
	;
	
	    [divaddr((sp,Offset))], !,
	    {
		Code = [swapd, const(65535),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,div]
	    }
	;
	    [divaddr((bp,Offset))], !,
	    {
		Code = [swapd, const(65534),swapa,load,swapd,swapa,const(Offset),add,swapa,swapd,load,swapd,div]
	    }
	;
	    [divaddr(Adr)],!,
	    {
		Code = [swapd, const(Adr),swapa,load,swapd,div]
	    }
	    
	
	;
	
	    [Other],!,
	    {
	      Code = [Other]
	    }
    ),
    translate(SextiumAssemblerRest, LabelCount,Labels),
    { append(Code, SextiumAssemblerRest, SextiumAssembler) }.
    
translate(SextiumAssembler,Labels,Labels) -->
    [],
    { SextiumAssembler = [] }.
    
translateFunctions(_,[],[]).  
translateFunctions(LabelsAcc,[(_,MacroAssemFunc) | Rest],SextiumFunc):-
  phrase(translate(SextiumAssemFunc,LabelsAcc,Labels),MacroAssemFunc),
  append(SextiumAssemFunc,SextiumAssemFuncRest,SextiumFunc),
  translateFunctions(Labels,Rest,SextiumAssemFuncRest).

%%%%%%%%%%%%%%%%%%%%%%   Uzupełnienie adresów   %%%%%%%%%%%%%%%%%%%%%%

completeAddr(SextiumNoAddr, SextiumAddr):-
  labelAddr(SextiumNoAddr, 0, 0, [], LabelsEnv),
  completeAddr(LabelsEnv, SextiumNoAddr, SextiumAddr).
  
% adresy labeli  
labelAddr([], _, _, Labels, Labels).  
labelAddr(Instructions, WordCounter, 4, Acc, Labels):-!,
    NewWordCounter is WordCounter + 1,
    labelAddr(Instructions, NewWordCounter, 0, Acc, Labels).
    
labelAddr([label(Nr, thunkconst(ConstNr)) | Rest], WordCounter, 0, Acc, Labels):- !,
    NewAcc = [(Nr, thunkconst(ConstNr), WordCounter) | Acc],
    NewWordCounter is WordCounter + 1,
    labelAddr(Rest, NewWordCounter, 0, NewAcc, Labels).  

labelAddr([label(Nr, thunkconst(ConstNr)) | Rest], WordCounter, _, Acc, Labels):-
    NewWordCounter is WordCounter + 1,
    labelAddr([label(Nr, thunkconst(ConstNr)) | Rest], NewWordCounter, 0, Acc, Labels).

labelAddr([label(Nr, thunkvar(ConstNr)) | Rest], WordCounter, 0, Acc, Labels):- !,
    NewAcc = [(Nr, thunkvar(ConstNr), WordCounter) | Acc],
    NewWordCounter is WordCounter + 1,
    labelAddr(Rest, NewWordCounter, 0, NewAcc, Labels).  
labelAddr([label(Nr, thunkvar(ConstNr)) | Rest], WordCounter, _, Acc, Labels):-
    NewWordCounter is WordCounter + 1,
    labelAddr([label(Nr, thunkvar(ConstNr)) | Rest], NewWordCounter, 0, Acc, Labels). 

labelAddr([label(Nr, Name) | Rest], WordCounter, 0, Acc, Labels):- !,
    NewAcc = [(Nr, Name, WordCounter) | Acc],
    labelAddr(Rest, WordCounter, 0, NewAcc, Labels).  
labelAddr([label(Nr, Name) | Rest], WordCounter, _, Acc, Labels):-
    NewWordCounter is WordCounter + 1,
    labelAddr([label(Nr, Name) | Rest], NewWordCounter, 0, Acc, Labels).  
    
labelAddr([const(_) | Rest], WordCounter, Buffer, Acc, Labels):-
    NewBuffer is Buffer + 1,
    NewWordCounter is WordCounter + 1,
    labelAddr(Rest, NewWordCounter, NewBuffer, Acc, Labels).
labelAddr([_ | Rest], WordCounter, Buffer, Acc, Labels):-
    NewBuffer is Buffer + 1,
    labelAddr(Rest, WordCounter, NewBuffer, Acc, Labels).

completeAddr(_, [], []).    
completeAddr(LabelsEnv, [const(label(Nr,Name)) | Rest], Completed):-!,
    findlabeladdr(LabelsEnv,(Nr,Name),Addr),
    Completed = [const(Addr) | RestCompleted],
    completeAddr(LabelsEnv, Rest, RestCompleted).
completeAddr(LabelsEnv, [store(label(Nr,Name)) | Rest], Completed):-!,
    findlabeladdr(LabelsEnv,(Nr,Name),Addr),
    Completed = [store(Addr) | RestCompleted],
    completeAddr(LabelsEnv, Rest, RestCompleted).
completeAddr(LabelsEnv, [Instr | Rest], Completed):-!,
    Completed = [Instr | RestCompleted],
    completeAddr(LabelsEnv, Rest, RestCompleted).

    
    
%%%%%%%%%%%%%%%%%%%%%%   Pakowanie instrukcji    %%%%%%%%%%%%%%%%%%%%%%
   
   
instructionPacking(NonPacked, Packed,CodeLength):-
    instructionPacking(NonPacked,[], 0, 0, Packed, CodeLength).

instructionPacking(Instructions, ConstAcc, WordCounter, 4, Packed, CodeLength):-!,
  NewWordCounter is WordCounter + 1,
  instructionPacking(Instructions, ConstAcc, NewWordCounter, 0, Packed, CodeLength).

instructionPacking([], [], CodeLength, 0, [], CodeLength):-!.
instructionPacking([], [], WordCounter, Buffer, Packed, CodeLength):-!,
    Packed = [nop | PackedRest],
    NewBuffer is Buffer + 1,
    instructionPacking([], [], WordCounter, NewBuffer, PackedRest, CodeLength).
  
instructionPacking(NonPacked, [Const | ConstAcc], WordCounter, 0, Packed, CodeLength):-!,
    Packed = [Const | PackedRest],
    NewWordCounter is WordCounter + 1,
    instructionPacking(NonPacked, ConstAcc, NewWordCounter, 0, PackedRest, CodeLength).
instructionPacking([], [Const | Rest], WordCounter, Buffer, Packed, CodeLength):-!,
  Packed = [nop | PackedRest],
  NewBuffer is Buffer + 1,
  instructionPacking([], [Const | Rest], WordCounter, NewBuffer, PackedRest, CodeLength).
    
instructionPacking([const(Val) | NonPacked], ConstAcc, WordCounter, Buffer, Packed, CodeLength):-!,
    Packed = [const | PackedRest],
    NewBuffer is Buffer + 1,
    append(ConstAcc,[Val],NewConstAcc),
    instructionPacking(NonPacked, NewConstAcc, WordCounter, NewBuffer, PackedRest, CodeLength).

instructionPacking([label(_,thunkvar(_)) | NonPacked], ConstAcc, WordCounter, 0, Packed, CodeLength):-!,
    NewWordCounter is WordCounter + 1,
    instructionPacking(NonPacked, ConstAcc, NewWordCounter, 0, Packed, CodeLength).    
instructionPacking([label(_,thunkconst(_)) | NonPacked], ConstAcc, WordCounter, 0, Packed, CodeLength):-!,
    NewWordCounter is WordCounter + 1,
    instructionPacking(NonPacked, ConstAcc, NewWordCounter, 0, Packed, CodeLength).    

instructionPacking([label(_,_) | NonPacked], ConstAcc, WordCounter, 0, Packed, CodeLength):-!,
    instructionPacking(NonPacked, ConstAcc, WordCounter, 0, Packed, CodeLength).    
instructionPacking([label(Nr,Name) | NonPacked], ConstAcc, WordCounter, Buffer, Packed, CodeLength):-!,
    Packed = [nop | PackedRest],
    NewBuffer is Buffer + 1,
    instructionPacking([label(Nr,Name) | NonPacked], ConstAcc, WordCounter, NewBuffer, PackedRest, CodeLength).
    
instructionPacking([Instr | NonPacked], ConstAcc, WordCounter, Buffer, Packed, CodeLength):-!,
    Packed = [Instr | PackedRest],
    NewBuffer is Buffer + 1,
    instructionPacking(NonPacked, ConstAcc, WordCounter, NewBuffer, PackedRest, CodeLength).
    
%%%%%%%%%%%%%%%%%%%%%%   Rozkazy -> liczby   %%%%%%%%%%%%%%%%%%%%%%

mnemonic(nop, hex(0)):-!.
mnemonic(syscall, hex(1)):-!.
mnemonic(load, hex(2)):-!.
mnemonic(store, hex(3)):-!.
mnemonic(swapa, hex(4)):-!.
mnemonic(swapd, hex(5)):-!.
mnemonic(branchz, hex(6)):-!.
mnemonic(branchn, hex(7)):-!.
mnemonic(jump, hex(8)):-!.
mnemonic(const, hex(9)):-!.
mnemonic(add, hex(10)):-!.
mnemonic(sub, hex(11)):-!.
mnemonic(mult, hex(12)):-!.
mnemonic(div, hex(13)):-!.
mnemonic(Other, Other):-!.

%%%%%%%%%%%%%%%%%%%%%%   Sextium object code   %%%%%%%%%%%%%%%%%%%%%%

sexObj([],[]).
sexObj([hex(First), hex(Second), hex(Third), hex(Fourth) | Rest], [Word | SextiumObjRest]):-!,
    Word is First*16^3 + Second*16^2 + Third*16 + Fourth,
    sexObj(Rest, SextiumObjRest).
sexObj([Const | Rest],[Const | SextiumObjRest]):-
  sexObj(Rest,SextiumObjRest).


%%%%%%%%%%%%%%%%%%%%%%   Kompilator   %%%%%%%%%%%%%%%%%%%%%%


algol16(Source, SextiumBin):-
  phrase(lexer(TokenList), Source),
  phrase(program(AST), TokenList),
  toMacroAssem(AST, MacroAssembler, Thunks, _, MacroLabelsCount, FunctionMacroAssembler),
  thunksMerge(Thunks, ThunksMerged, ThunkLabels),
  append(MacroAssembler, ThunksMerged, MacroAssemWithThunks),
  phrase(translate(SextiumMainNoAdr, MacroLabelsCount, MainLabels), MacroAssemWithThunks),
  translateFunctions(MainLabels, FunctionMacroAssembler, SextiumFuncNoAdr),
  concat([SextiumMainNoAdr,SextiumFuncNoAdr, ThunkLabels], SextiumNoAdr),
  completeAddr(SextiumNoAdr, SextiumAdr),
  instructionPacking(SextiumAdr, SextiumMnem, _),
  maplist(mnemonic,SextiumMnem, SextiumHex),
  sexObj(SextiumHex,SextiumBin).


%%%%%%%%%%%%%%%%%%%%%%   Pomocnicze   %%%%%%%%%%%%%%%%%%%%%%

concat([H|T],List):-
	concat([H|T],[],List).
concat([],List,List).
concat([H|T],Acc,List):-
	append(Acc,H,NewAcc),
	concat(T,NewAcc,List).

findvar([(X1,Offset,Scope,Type)|_],X1,(X1,Offset,Scope,Type)):-!.
findvar([_|Rest],X1,(X1,Offset,Scope,Type)):-
  findvar(Rest,X1,(X1,Offset,Scope,Type)).
  
findlabeladdr([(Nr, Name, Addr) | _], (Nr,Name), Addr):-!.
findlabeladdr([_ | Rest], (Nr,Name), Addr):-
  findlabeladdr(Rest, (Nr,Name), Addr).
  
findproc([(Name,ArgList,Declarations,Instructions)|_],Name,(Name,ArgList,Declarations,Instructions)):-!.
findproc([_|Rest],Name,(Name,ArgList,Declarations,Instructions)):-
  findproc(Rest,Name,(Name,ArgList,Declarations,Instructions)).


thunksMerge(Thunks, ThunksMerged, ThunkLabels):-
  thunksMerge(Thunks, [], ThunkLabels, ThunksMerged).

thunksMerge([], Labels, Labels, []).


thunksMerge([(ThunkCode, ThunkNr, ThunkLabels) | Rest], LabelsAcc, Labels, ThunksMerged):-
    concat([[label(_, thunk(ThunkNr))], ThunkCode, [pop,store(65533),pop,jump]], ThunkFinalCode),
    append(ThunkFinalCode,ThunksMergedRest,ThunksMerged),
    append(LabelsAcc, ThunkLabels, NewLabelsAcc),
    thunksMerge(Rest, NewLabelsAcc, Labels, ThunksMergedRest).
