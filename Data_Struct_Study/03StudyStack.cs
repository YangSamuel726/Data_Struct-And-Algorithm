using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

// 작성자 : 양사무엘
// 작성일자 : 2026-09-18
// 공부 주제 : 스택(Stack)
// 목표 : Stack의 특징과 장단점

class StudyStack
{
    static void Main()
    {
        // TestStack();
        TestMyStack();
    }

    static void TestStack()
    {
        Stack<int> stack = new Stack<int>();

        // Error! 스택에 요소가 없는 상태에서 Pop을 하면 InvalidOperationException 예외가 발생한다.
        // stack.Pop();

        // trySuccess = false, result = 0 | TryPop은 요소가 없을때 예외 대신 bool값으로 성공 여부를 알려주고 성공여부에 따라 값이나 데이터 타입의 기본값을 준다.
        bool trySuccess = stack.TryPop(out int result);
        Console.WriteLine($"빈 스택에서 요소를 꺼내는데 성공했나요? => {trySuccess} / 값이 어떻게 되나요? => {result}");
    }

    static void TestMyStack()
    {
        MyStack<int> myStack = new MyStack<int>();
        myStack.Push(1);
        myStack.Push(2);
        // 2, 2, 1을 차례대로 반환
        Console.WriteLine($"맨 위에 있는 요소 : {myStack.Peek()}");
        Console.WriteLine($"꺼낸 요소 : {myStack.Pop()}");
        Console.WriteLine($"꺼낸 요소 : {myStack.Pop()}");

        // Error! InvalidOperationException
        Console.WriteLine($"꺼낸 요소 : {myStack.Pop()}");
    }
}

// [스택이란?]
// 스택은 마지막에 들어간 요소가 처음으로 나오는(Last In First Out, LIFO) 원칙을 따르는 선형 데이터 구조이다.
// 스택에 추가된 마지막 요소가 가장 먼저 제건된다.

// [특징]
// LIFO 원칙 : 스택에 푸시된 마지막 요소가 가장 먼저 팝된다.
// 동적크기 : 스택은 요소의 수에 따라 늘어나거나 줄어들 수 있다.
// 제한된 접근 : 최상단 요소만 직접 접근할 수 있으며, 다른 요소에 접근하려면 최상단 요소를 팝해야 한다.

// [일반적인 스택 연산]
// 푸시(Push) : 스택의 최상단에 요소를 추가한다.
// 팝(Pop) : 스택에서 최상단 요소를 제거하고 반환한다.
// 피크(Peek) : 스택에서 최상단 요소를 제거하지 않고 반환한다.

// [C#(.Net)에서의 스택 구현]
// 스택은 배열이나 연결 리스트를 사용하여 구현할 수 있지만, .Net Framework에 구현을 간소화하는 내장 클래스인 Stack<T>를 제공한다.

// [질문]
// 1. 스택의 내부 구현 베이스를 배열말고 연결 리스트로 만들면 어떻게 될까?

// [직접 구현해보기]
// 구현할 때 Stack의 특징을 다 가지도록 구현할 것.
#region Array로 Stack구현
public class MyStack<T>
{
    private const int DefaultCapacity = 4;
    private T[] _array;
    private int _size;

    public int Count => _size;

    public MyStack()
    {
        _array = Array.Empty<T>();
    }
    public MyStack(int Capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
        _array = new T[Capacity];
    }

    // 다음 사이즈 값의 위치가 배열의 크기를 벗어나는지 확인
    // 벗어나면 배열 사이즈 재 조정 후 값 할당
    // 벗어나지 않으면 값 할당
    // 이후 사이즈 값을 +1
    public void Push(T value)
    {
        int size = _size;
        int arrLength = _array.Length;
        if (size >= arrLength)
        {
            ResizeArray();
        }
        _array[size] = value;
        size++;
        _size = size;
    }

    // 꺼내야할 위치의 인덱스를 사이즈를 기반으로 가져오기
    // 인덱스가 음수라면 InvalidOperationException 던지기
    // 값을 가져온 다음에 가져온 위치의 값의 타입이 참조형이나 참조를 포함한 구조체면 기본값 초기화
    // 사이즈 조정 후 값 반환
    public T Pop()
    {
        int size = _size - 1;
        if (size < 0)
        {
            throw new InvalidOperationException("꺼낼 요소가 존재하지 않습니다.");
        }
        T value = _array[size];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _array[size] = default!;
        }
        _size = size;
        return value;
    }
    public bool TryPop(out T value)
    {
        value = default!;
        int size = _size - 1;
        if (size < 0)
        {
            return false;
        }
        value = _array[size];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _array[size] = default!;
        }
        _size = size;
        return true;
    }

    // Pop과 절차는 동일하지만 다른것은 사이즈를 조정하지 않고 참조형이어도 확인한 배열의 위치를 비우지 않음
    public T Peek()
    {
        int size = _size - 1;
        if (size < 0)
        {
            throw new InvalidOperationException("확인할 요소가 존재하지 않습니다.");
        }
        T value = _array[size];
        return value;
    }
    public bool TryPeek(out T value)
    {
        value = default!;
        int size = _size - 1;
        if (size < 0)
        {
            return false;
        }
        value = _array[size];
        return true;
    }

    // 배열의 크기를 조절함. 0이었으면 기본 크기값으로. 그 외에는 기존 크기의 2배로 설정함
    // 만약에 배열 최대 크기를 넘어가면 최대 크기로 다시 조정함.
    private void ResizeArray()
    {
        int newCapacity = _array.Length == 0 ? DefaultCapacity : _array.Length * 2;
        if ((uint)newCapacity > Array.MaxLength)
        {
            newCapacity = Array.MaxLength;
        }
        Array.Resize(ref _array, newCapacity);
    }

    // [느낀점]
    // Stack을 직접 만들면서 Push, Pop, Peek가 왜 O(1)인지 알 수 있었다.
    // 먼저 Push는 현재 내부 배열의 용량이 가득 차면 넘어서기 전까지는 O(1)으로 작동하다가 최대 크기에 도달하면 배열의 크기를 늘리면서 O(n) 시간복잡도를 가지게 된다. 그러나 여러 번의 연산 비용을 합산하면, 한 번당 분할 상환 비용은 O(1)이 된다.
    // Pop과 Peek는 값을 꺼내올때 _size를 사용하여 배열에 인덱스로 접근하여 가져온다. 그렇기에 O(1) 시간복잡도를 가진다.

    // [궁금한 점]
    // .Net의 기본 Stack구현에서는 size나 Capacity 등 int변수앞에 (uint)형변환을 해주던데 왜 하는 것일까?
}
#endregion