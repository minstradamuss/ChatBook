# Паттерны и SOLID-принципы в проекте ChatBook

## Архитектура проекта
![image](https://github.com/user-attachments/assets/2a70b845-0929-4853-8868-2722b825a763)


## Применённые паттерны проектирования

### 1. Strategy
**Где используется:**  
`TokenService` использует стратегию `ITokenStrategy` для генерации токена. Это позволяет при необходимости подменить реализацию JWT на другую (например, OAuth).

**Принцип:**  
Вынос алгоритма генерации токена в отдельную стратегию для гибкости и тестируемости.

---

### 2. Repository
**Где используется:**  
`IUserRepository`, `IBookRepository`, `IMessageRepository`, `FriendshipRepository` абстрагируют доступ к данным.  
Каждому из них соответствует реализация, работающая с Entity Framework / SQLite.

**Принцип:**  
Чёткое разделение между слоем доступа к данным и бизнес-логикой.

---

### 3. Dependency Injection (DI)
**Где используется:**  
Весь проект основан на внедрении зависимостей через конструкторы:  
- контроллеры (например, `AuthController`)
- сервисы (например, `UserService`, `BookService`)
- UI слои через `Program.cs` и `ServiceProvider`.

**Принцип:**  
Позволяет изолировать зависимости, упростить тестирование и замену реализаций.

---

### 4. MVVM (в WPF части)
**Где используется:**  
В `LoginViewModel`, `MainViewModel`, `AddBookViewModel` и других.  
Отделение представления (`View`) от логики (`ViewModel`), что упрощает логику UI и тестирование.

---

### 4. Factory Pattern
**Где используется:**
IBookFactory и реализация BookFactory позволяют централизованно и гибко создавать экземпляры книг.

**Зачем:**
Создание книги теперь не зависит от new Book { ... } в каждом месте. Удобно при множестве параметров и/или логике инициализации.

---

## Принципы SOLID

### S — Single Responsibility Principle (SRP)
Каждый класс отвечает только за одну зону ответственности:
- `UserService` — работа с пользователем
- `BookService` — работа с книгами
- `ChatService` — работа с сообщениями

---

### O — Open/Closed Principle (OCP)
Сервисы и интерфейсы можно расширять, не изменяя существующий код.  
Например, можно добавить новую реализацию `ITokenStrategy`, не трогая `AuthController`.

---

### L — Liskov Substitution Principle (LSP)
Интерфейсы (`IUserRepository`, `IBookRepository`, `ITokenStrategy`) позволяют заменять реализации без изменения потребителей.

---

### I — Interface Segregation Principle (ISP)
Интерфейсы разбиты по зонам ответственности:  
- `IUserService` — только логика пользователя  
- `IBookService` — только книги  
- `IChatService` — только чат

---

### D — Dependency Inversion Principle (DIP)
Вместо жёстких зависимостей используются интерфейсы:  
`UserService` зависит от `IUserRepository`, а не от конкретной реализации.

---
