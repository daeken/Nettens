; ModuleID = 'struct.c'
source_filename = "struct.c"
target datalayout = "e-m:o-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-apple-macosx10.13.0"

%struct.foo_s = type { i32, i32, i8*, %struct.foo_s* }

; Function Attrs: noinline nounwind optnone ssp uwtable
define void @test() #0 {
  %1 = alloca %struct.foo_s, align 8
  %2 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 0
  store i32 5, i32* %2, align 8
  %3 = bitcast %struct.foo_s* %1 to i8*
  %4 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 2
  store i8* %3, i8** %4, align 8
  %5 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 2
  %6 = load i8*, i8** %5, align 8
  %7 = bitcast i8* %6 to %struct.foo_s*
  %8 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %7, i32 0, i32 0
  %9 = load i32, i32* %8, align 8
  %10 = add i32 %9, 6
  %11 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 1
  store i32 %10, i32* %11, align 4
  %12 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 2
  %13 = load i8*, i8** %12, align 8
  %14 = bitcast i8* %13 to %struct.foo_s*
  %15 = getelementptr inbounds %struct.foo_s, %struct.foo_s* %1, i32 0, i32 3
  store %struct.foo_s* %14, %struct.foo_s** %15, align 8
  ret void
}

attributes #0 = { noinline nounwind optnone ssp uwtable "correctly-rounded-divide-sqrt-fp-math"="false" "disable-tail-calls"="false" "less-precise-fpmad"="false" "no-frame-pointer-elim"="true" "no-frame-pointer-elim-non-leaf" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="penryn" "target-features"="+cx16,+fxsr,+mmx,+sse,+sse2,+sse3,+sse4.1,+ssse3,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"clang version 5.0.1 (tags/RELEASE_501/final)"}
