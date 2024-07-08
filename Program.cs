using System.Collections;

namespace ToDoListApp{
    //Задача
    public class Task{
        public int Id{get; set;}
        public string Description{get; set;}
        public bool IsCompleted{get; set;}
        public Task(int id, string description){
            Id = id;
            Description = description;
            IsCompleted = false;
        }
        public override string ToString(){
            return $"{Id}: {Description} - {(IsCompleted ? "Выполнено" : "Не выполнено")}";
        }
    }
    //Управляет задачами
    public class TaskManager{
        private List<Task> tasks;
        private int nextId;
        public TaskManager(){
            tasks = new List<Task>();
            nextId = 1;
        }
        //Добавление задачи
        public void AddTask(string description){
            tasks.Add(new Task(nextId++, description));
        }
        //Просмотр задач
        public void ViewTasks(){
            foreach(var task in tasks) 
                Console.WriteLine(task);
        }
        //Отметка задачи как выполненной
        public void CompletedTask(int id){
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if(task!=null){
                task.IsCompleted = true;
            }
            else Console.WriteLine("Задача не найдена.");
        }
        //Удаление задачи
        public void DelTask(int id){
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if(task!=null){
                tasks.Remove(task);
            }
            else Console.WriteLine("Задача не найдена.");
        }
        //Фильтрация по статусу
        public void FilterTasks(bool showCompleted){
            var filteredTasks = tasks.Where(t => t.IsCompleted == showCompleted);
            foreach(var task in filteredTasks)
                Console.WriteLine(task);
        }
        //Сохранение в файл
        public void SaveInFile(string fileName){
            using (var writer = new StreamWriter(fileName)){
                foreach(var task in tasks)
                    writer.WriteLine($"{task.Id}|{task.Description}|{task.IsCompleted}");
            }
        }
        //Загрузка задач из файла
        public void DownloadFromFile(string fileName){
            if(File.Exists(fileName)){
                tasks.Clear();
                var lines = File.ReadAllLines(fileName);
                foreach (var line in lines){
                    var parts = line.Split('|');
                    var task = new Task(int.Parse(parts[0]), parts[1]){
                        IsCompleted = bool.Parse(parts[2])
                    };
                    tasks.Add(task);
                    nextId = Math.Max(nextId, task.Id + 1);
                }
            }
        }
    }
    class Program{
        static void Main(string[] args){
            var taskManager = new TaskManager();
            const string fileName = "tasks.txt";
            //Загрузка из файла
            taskManager.DownloadFromFile(fileName);
            while(true){
                Console.WriteLine("1) Добавить задачу");
                Console.WriteLine("2) Просмотр спиcка задач");
                Console.WriteLine("3) Отметить задачу как выполненную");
                Console.WriteLine("4) Удалить задачу");
                Console.WriteLine("5) Фильтр задач (1 - Выполнено, 0 - Невыполнено)");
                Console.WriteLine("6) Выгрузить в файл и выйти");

                var input = Console.ReadLine();
                if(input == "1"){
                    Console.Write("Введите описание задачи: ");
                    var description = Console.ReadLine();
                    taskManager.AddTask(description);

                }
                else if(input == "2"){
                    taskManager.ViewTasks();
                }
                else if(input == "3"){
                    Console.Write("Введите ID задачи для отметки как выполненной: ");
                    if(int.TryParse(Console.ReadLine(), out int id)){
                        taskManager.CompletedTask(id);
                    }
                    else{
                        Console.WriteLine("Некорректный Id");
                    }
                }
                else if(input == "4"){
                    Console.Write("Введите ID задачи для удаления: ");
                    if(int.TryParse(Console.ReadLine(), out int id)){
                        taskManager.DelTask(id);
                    }
                    else{
                        Console.WriteLine("Некорректный Id");
                    }
                }
                else if(input == "5"){
                    Console.WriteLine("Показать выполненные (1) или невыполненные (0) задачи? ");
                    if(int.TryParse(Console.ReadLine(), out int filterOption) && (filterOption == 0 || filterOption == 1)){
                        bool showCompleted = filterOption == 1;
                        taskManager.FilterTasks(showCompleted);
                    }
                    else Console.WriteLine("Некорректный ввод");
                }
                else if(input == "6"){
                    taskManager.SaveInFile(fileName);
                    break;
                }
                else Console.WriteLine("Некорректный ввод. Попробуйте снова.");
            }
        }
    }
}