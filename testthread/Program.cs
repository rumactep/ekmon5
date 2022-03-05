using System;
using System.Threading;
namespace testthread;

class WorkClass {
    int Data;
    readonly ManualResetEvent _mainExitEvent;
    readonly ManualResetEvent _workEndedEvent;
    public WorkClass(int data, ManualResetEvent mainExitEvent, ManualResetEvent workEndedEvent) {
        Data = data;
        _mainExitEvent = mainExitEvent;
        _workEndedEvent = workEndedEvent;
    }
    public void ThreadProc() {
        Console.WriteLine($"thread {Data} started");
        var r = new Random();
        do {
            Thread.Sleep(2000); // очень важная работа          
        }
        while (!_mainExitEvent.WaitOne(r.Next(2000)));
        Console.WriteLine($"thread {Data} stopped");
        _workEndedEvent.Set(); // событие того, что работа зкончена
    }
}

public class Program {
    public static void Main(string[] args) {
        ManualResetEvent mainExitEvent = new(false);
        const int THREAD_COUNT = 10;
        ManualResetEvent [] workEndEvents = new ManualResetEvent[THREAD_COUNT];
        for (int i = 0; i < THREAD_COUNT; i++) {
            workEndEvents[i] = new ManualResetEvent(false);
            WorkClass work = new WorkClass(i, mainExitEvent, workEndEvents[i]);
            Thread workThread = new Thread(new ThreadStart(work.ThreadProc));
            workThread.Start();
        }
        Thread.Sleep(10);
        Console.WriteLine("press any key to exit from main thread");
        Console.ReadKey();  // по какому-то условию заканчиваем работу
        Console.WriteLine("trying to exit:");
        mainExitEvent.Set();
        Thread.Sleep(10);
        WaitHandle.WaitAll(workEndEvents);
        Console.WriteLine("All exited!");
    }
}
