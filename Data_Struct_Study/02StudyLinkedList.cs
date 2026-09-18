using System;
using System.Diagnostics;

// 작성자 : 양사무엘
// 작성일자 : 2026-09-17
// 공부 주제 : 연결리스트(LinkedList)
// 목표 : 연결리스트의 특징과 장단점 이해

class StudyLinkedList
{
    static void Main()
    {
        // AddOneNodeCheckFirstLast();
        // TestMyLinkedList();
        LinkedListForeachTest();
    }

    // 노드를 한개만 추가하면 first와 last가 같을까?
    static void AddOneNodeCheckFirstLast()
    {
        LinkedList<int> linkList = new LinkedList<int>();
        linkList.AddLast(5);

        // true
        Console.WriteLine($"첫 노드와 마지막 노드가 같나요? : {linkList.First == linkList.Last}");
    }

    static void LinkedListForeachTest()
    {
        LinkedList<int> linkList = new LinkedList<int>();
        for (int i = 0; i < 10; i++) { linkList.AddLast(i); }

        // Error!!!!
        // foreach (var value in linkList)
        // {
        //     if (value % 2 == 0)
        //     {
        //         linkList.AddLast(linkList.Last!.Value + 1);
        //     }
        //     Console.WriteLine($"{value}번째 노드");
        // }

        // 19까지 추가됨
        var node = linkList.First;
        while (node != null)
        {
            var next = node.Next; // 현재 노드가 삭제되면 다음 참조가 사라지니 미리 참조 캐싱
            if (node.Value % 2 == 0)
            {
                linkList.AddLast(linkList.Last!.Value + 1);
            }
            Console.WriteLine($"{node.Value}번째 노드");
            node = next;
        }
    }

    static void TestMyLinkedList()
    {
        MyLinkedList<int> myLinkList = new MyLinkedList<int>();
        myLinkList.AddLast(5);
        Console.WriteLine($"첫 노드의 값 : {myLinkList.First!.value}");

        myLinkList.AddBefore(myLinkList.First, 10);
        Console.WriteLine($"마지막 노드의 값 : {myLinkList.Last!.value}");
    }
}

// 1. 연결 리스트란?
// 각 노드가 데이터와 다른 노드에 대한 참조를 저장하고, 이 참조로 노드들을 연결하여 순서를 구성하는 자료구조이다. 단일 연결 리스트는 다음 노드의 참조를, 이중 연결 리스트는 이전 노드와 다음 노드의 참조를 가진다. C#(.Net)의 기본 제공되어지는 LinkedList는 이중 연결 리스트이다.
// 첫번재 노드는 '머리(head)'라고 하며, 마지막 노드는 '꼬리(tail)'라고 한다. 마지막 노드의 내부 next는 head를 가리키지만, 공개 Next는 null을 반환한다.

// 2. 특징
// 동적 크기 : 미리 총 요소 수를 지정할 필요가 없다.
// 비연속 메모리 : 참조 사용으로 인해 요소가 흩어져 있는 메모리에 저장될 수 있다.
// 효율성 : 삽입 및 삭제 작업이 효율적으로 수행된다.

// 3. 장점
// 동적 크기 : 필요한 만큼 노드를 추가하거나 제거할 수 있으며, 크기를 늘리기 위해 기존 요소 전체를 새로운 저장 공간으로 복사할 필요가 없다.
// 삽입·삭제 용이성 : 필요한 노드 참조가 주어지면 연결 변경은 O(1), 탐색이 필요하면 그 비용이 추가된다.

// 4. 단점
// 메모리 오버헤드 : 각 노드의 포인터(참조)로 인해 추가 메모리를 소비한다.
// 순차 접근 : 배열과 달리 인덱스를 통해 노드에 직접 접근할 수 없어서 노드를 검색할 때 O(n) 시간 복잡도가 발생한다.

// 5. 사용 선호 상황
// 빈번한 삽입 및 삭제
// 동적 데이터 크기
// 복잡한 데이터 구조
// 중간 삽입

// 6. C#(.Net)의 LinkedList
// C#의 연결 리스트는 원형 이중 연결 리스트이다. 외부에서 보기에는 이중 연결 리스트로 보이지만 내부적으로는 삽입·삭제 로직을 일관성있게 하기 위해 머리와 꼬리가 서로를 참조하여 원형으로 이루어져 있다.

//[직접 구현해보기]
// 구현할 때 특징을 포함하도록 구현하기
public class MyLinkedList<T>
{
    internal MyLinkedListNode<T>? head;
    internal int count = 0;

    public MyLinkedListNode<T>? First
    {
        get => head;
    }

    public MyLinkedListNode<T>? Last
    {
        get => head?.prev;
    }

    public int Count
    {
        get => count;
    }

    public void AddLast(T value)
    {
        MyLinkedListNode<T> newNode = new MyLinkedListNode<T>(value, this);
        if (head == null) InternalAddNodeToEmptyList(newNode);
        else InternalAddBeforeNode(head, newNode);
    }
    public void AddLast(MyLinkedListNode<T> newNode)
    {
        ValidateNewNode(newNode);
        if (head == null) InternalAddNodeToEmptyList(newNode);
        else InternalAddBeforeNode(head, newNode);
        newNode.list = this;
    }

    public void AddFirst(T value)
    {
        MyLinkedListNode<T> newNode = new MyLinkedListNode<T>(value, this);

        if (head == null)
        {
            InternalAddNodeToEmptyList(newNode);
        }
        else
        {
            InternalAddBeforeNode(head, newNode);
            head = newNode;
        }
    }
    public void AddFirst(MyLinkedListNode<T> newNode)
    {
        ValidateNewNode(newNode);
        if (head == null)
        {
            InternalAddNodeToEmptyList(newNode);
        }
        else
        {
            InternalAddBeforeNode(head, newNode);
            head = newNode;
        }
        newNode.list = this;
    }

    public void AddBefore(MyLinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        MyLinkedListNode<T> newNode = new MyLinkedListNode<T>(value, node.list!);
        InternalAddBeforeNode(node, newNode);
        if (head == node)
        {
            head = newNode;
        }
    }
    public void AddBefore(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
    {
        ValidateNode(node);
        ValidateNewNode(newNode);
        InternalAddBeforeNode(node, newNode);
        if (head == node)
        {
            head = newNode;
        }
        newNode.list = this;
    }

    public void AddAfter(MyLinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        MyLinkedListNode<T> newNode = new MyLinkedListNode<T>(value, node.list!);
        InternalAddBeforeNode(node.next!, newNode);
    }
    public void AddAfter(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
    {
        ValidateNode(node);
        ValidateNewNode(newNode);
        InternalAddBeforeNode(node.next!, newNode);
        newNode.list = this;
    }

    public void RemoveNode(MyLinkedListNode<T> node)
    {
        ValidateNode(node);
        InternalRemoveNode(node);
    }

    internal void InternalAddNodeToEmptyList(MyLinkedListNode<T> newNode)
    {
        Debug.Assert(head == null && count == 0, "이 함수는 리스트의 첫 노드를 추가할 때 호출되어야 해요!");
        newNode.prev = newNode;
        newNode.next = newNode;
        head = newNode;
        count++;
    }

    internal void InternalAddBeforeNode(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
    {
        newNode.prev = node.prev;
        newNode.next = node;
        node.prev!.next = newNode;
        node.prev = newNode;
        count++;
    }

    internal void InternalRemoveNode(MyLinkedListNode<T> node)
    {
        if (count == 1)
        {
            head = null;
        }
        else
        {
            if (node == head)
            {
                head = node.next;
            }
            node.prev!.next = node.next;
            node.next!.prev = node.prev;
        }
        node.Invalidate();
        count--;
    }

    private void ValidateNode(MyLinkedListNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);
        if (node.list != this)
        {
            throw new InvalidOperationException("노드가 현재 연결 리스트에 속해 있지 않습니다.");
        }
    }
    private void ValidateNewNode(MyLinkedListNode<T> newNode)
    {
        ArgumentNullException.ThrowIfNull(newNode);
        if (newNode.list != null)
        {
            throw new InvalidOperationException("새로운 노드가 이미 다른 리스트에 속해 있습니다.");
        }
    }
}

public sealed class MyLinkedListNode<T>
{
    internal MyLinkedList<T>? list;
    internal MyLinkedListNode<T>? prev;
    internal MyLinkedListNode<T>? next;
    public T? value;

    public MyLinkedListNode() { }
    internal MyLinkedListNode(T value, MyLinkedList<T> list)
    {
        this.value = value;
        this.list = list;
    }
    public MyLinkedListNode(T value)
    {
        this.value = value;
    }

    public MyLinkedListNode<T>? Next
    {
        get => next == null || list!.head == next ? null : next;
    }

    public MyLinkedListNode<T>? Previous
    {
        get => prev == null || list!.head == this ? null : prev;
    }

    internal void Invalidate()
    {
        list = null;
        prev = null;
        next = null;
    }
}