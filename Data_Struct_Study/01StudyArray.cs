using System;
using System.Text;

// 작성자 : 양사무엘
// 작성일자 : 2026-09-16
// 공부 주제 : 배열(Array)
// 목표 : 배열의 특징과 장단점 이해

class StudyArray
{
    static void Main()
    {
        ArrayInitTest();
        ArrayElementValueInitTest();
    }

    static void ArrayInitTest()
    {
        int[] arr1 = new int[5]; // 선언과 할당만하고 초기값 부여는 하지 않음
        int[] arr2 = new int[5] { 1, 2, 3, 4, 5 }; // 선언과 동시에 할당 및 초기화
        int[] arr3 = { 1, 2, 3, 4, 5 }; // 초기화 값 개수를 통해 배열 크기 결정

        Console.WriteLine($"arr1의 배열 크기 : {arr1.Length}");
        Console.WriteLine($"arr2의 배열 크기 : {arr2.Length}");
        Console.WriteLine($"arr3의 배열 크기 : {arr3.Length}");
    }

    static void ArrayElementValueInitTest()
    {
        StringBuilder sbForArr = new StringBuilder();

        // 값형을 받는 배열
        int[] arr = new int[3];
        for (int i = 0; i < arr.Length; i++)
        {
            sbForArr.Append($"{arr[i]} ");
        }
        Console.WriteLine($"값형을 저장하는 배열 내부 요소들의 값 : {sbForArr}");


        sbForArr.Clear();

        // 참조형을 받는 배열
        SomeRefObject[] refArr = new SomeRefObject[3];
        for (int i = 0; i < refArr.Length; i++)
        {
            string appendText = refArr[i] == null ? "Null" : "NotNull";
            sbForArr.Append($"{appendText} ");
        }
        Console.WriteLine($"참조형을 저장하는 배열 내부 요소들의 값 : {sbForArr}");
    }

    private class SomeRefObject
    {
        
    }
}

// [배열이란?]
// 연속된 메모리 위치에 저장된 항목의 모음이다. 단일 유형의 고정된 수의 값을 보유하는 (선형)자료구조이다.

// [특징]
// 고정 크기 : 배열의 크기는 생성 시에 정의된다.
// 제로 기반 인덱싱 : 첫 요소는 인덱스 0부터 시작하여 접근한다.
// 동종 데이터 유형 : 배열의 모든 요소는 동일한 데이터 유형이어야 한다.
// 기본값 할당 : 배열 생성 시 별도의 초기값을 지정하지 않은 요소는 해당 타입의 기본값으로 초기화된다.


// [장점]
// 조회 : 인덱스를 통해 O(1)의 속도로 조회가 가능하다.
// CPU 캐시 : 순서대로 처리할 때 CPU캐시를 활용하기 좋다. (공간 지역성)

// [단점]
// 고정 크기 : 배열의 크기를 넘어서 요소를 추가하려고 하면 새로운 배열을 선언하여 요소를 복사하여 만들어야 한다. 또 완전히 활용되지 않는 경우 낭비되는 공간이 발생한다.
// 중간 삽입·삭제의 이동 비용 : 원소의 순서를 유지하면서 중간에 삽입하거나 삭제하려면 뒤쪽 원소들을 이동해야 하므로 최악의 경우 O(n)이 걸린다.