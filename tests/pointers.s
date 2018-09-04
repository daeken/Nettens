; ModuleID = 'pointers.c'
source_filename = "pointers.c"
target datalayout = "e-m:e-p:32:32-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i386-pc-none-eabi"

; Function Attrs: noinline nounwind optnone
define i32 @deref(i8*) #0 {
  %2 = alloca i8*, align 4
  %3 = alloca i32, align 4
  store i8* %0, i8** %2, align 4
  %4 = load i8*, i8** %2, align 4
  %5 = bitcast i8* %4 to i32*
  %6 = load i32, i32* %5, align 4
  store i32 %6, i32* %3, align 4
  %7 = load i32, i32* %3, align 4
  %8 = add i32 %7, 42
  ret i32 %8
}

; Function Attrs: noinline nounwind optnone
define i32 @test() #0 {
  %1 = alloca i8*, align 4
  store i8* inttoptr (i32 -559038737 to i8*), i8** %1, align 4
  %2 = load i8*, i8** %1, align 4
  %3 = call i32 @deref(i8* %2)
  ret i32 %3
}

attributes #0 = { noinline nounwind optnone "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "less-precise-fpmad"="false" "no-frame-pointer-elim"="true" "no-frame-pointer-elim-non-leaf" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"NumRegisterParameters", i32 0}
!1 = !{i32 1, !"wchar_size", i32 4}
!2 = !{!"clang version 6.0.1-svn334776-1~exp1~20180826122732.96 (branches/release_60)"}
