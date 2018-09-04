; ModuleID = 'pointers.c'
source_filename = "pointers.c"
target datalayout = "e-m:e-p:32:32-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i386-pc-none-eabi"

; Function Attrs: norecurse nounwind readonly
define i32 @deref(i8* nocapture readonly) local_unnamed_addr #0 {
  %2 = bitcast i8* %0 to i32*
  %3 = load i32, i32* %2, align 4, !tbaa !3
  %4 = add i32 %3, 42
  ret i32 %4
}

; Function Attrs: norecurse nounwind readonly
define i32 @test() local_unnamed_addr #0 {
  %1 = tail call i32 @deref(i8* inttoptr (i32 -559038737 to i8*))
  ret i32 %1
}

attributes #0 = { norecurse nounwind readonly "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "less-precise-fpmad"="false" "no-frame-pointer-elim"="true" "no-frame-pointer-elim-non-leaf" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"NumRegisterParameters", i32 0}
!1 = !{i32 1, !"wchar_size", i32 4}
!2 = !{!"clang version 6.0.1-svn334776-1~exp1~20180826122732.96 (branches/release_60)"}
!3 = !{!4, !4, i64 0}
!4 = !{!"int", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C/C++ TBAA"}
