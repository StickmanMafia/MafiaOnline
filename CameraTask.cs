public class CameraTask
{
    fn main() {
        // `print!`, как `println!`, но он не добавляет новую строку в конце
        print!("January has ");

        // `{}` это заполнители для аргументов, которые будут строками
        println!("{} days", 31i);
        // `i` суффикс указывает компилятору, что этот литерал имеет тип: целое
        // число со знаком, смотрите следующую главу для более подробной информации

        // Позиционные аргументы могут быть повторно использованы по шаблону
        println!("{0}, this is {1}. {1}, this is {0}", "Alice", "Bob");

        // Аргументы можно называть
        println!("{subject} {verb} {predicate}",
            predicate="over the lazy dog",
            subject="the quick brown fox",
            verb="jumps");

        // Специальное форматирование может быть указано в заполнителе после `:`, `t` это бинарное представление
        println!("{} of {:t} people know binary, the other half don't", 1i, 2i);

        // Ошибка! Не хватает аргумента для вывода
        println!("My name is {0}, {1} {0}", "Bond");
        // ИСПРАВЬТЕ ^ добавьте отсутствующий аргумент: "James"
    }
    
    fn main() {
        // Целочисленное сложение
        println!("1 + 2 = {}", 1u + 2);

        // Вычитание
        println!("1 - 2 = {}", 1i - 2);
        // Попробуйте изменить `1i` на `1u` и понять, почему тип важен

        // Булева логика
        println!("true AND false is {}", true && false);
        println!("true OR false is {}", true || false);
        println!("NOT true is {}", !true);

        // Битовые операции
        println!("0011 AND 0101 is {:04t}", 0b0011u & 0b0101);
        println!("0011 OR 0101 is {:04t}", 0b0011u | 0b0101);
        println!("0011 XOR 0101 is {:04t}", 0b0011u ^ 0b0101);
        println!("1 << 5 is {}", 1u << 5);
        println!("0x80 >> 2 is 0x{:x}", 0x80u >> 2);

        // Используйте подчеркивания, чтобы улучшить читаемость
        println!("One million is written as {}", 1_000_000u);
    }
    
    fn main() {
        let an_integer = 1u;
        let a_boolean = true;
        let unit = ();

        // скопировать значение `an_integer` в `copied_integer`
        let copied_integer = an_integer;

        println!("An integer: {}", copied_integer);
        println!("A boolean: {}", a_boolean);
        println!("Meet the unit value: {}", unit);

        // Компилятор предупреждает о неиспользуемых переменных; эти предупреждения можно
        // отключить используя подчёркивание перед именем переменной
        let _unused_variable = 3u;
        let noisy_unused_variable = 2u;
        // ИСПРАВЬТЕ ^ Добавьте подчёркивание
    }

}

fn main() {
let _immutable_variable = 1i;
let mut mutable_variable = 1i;

println!("Before mutation: {}", mutable_variable);

// Ок
mutable_variable += 1;

println!("After mutation: {}", mutable_variable);

// Ошибка!
_immutable_variable += 1;
}

fn main() {
// Эта переменная живет в области функции main
let long_lived_variable = 1i;

// Это блок, он имеет меньший объем нежели основная функция
{
    // Эта переменная существует только в этом блоке
    let short_lived_variable = 2i;

    println!("inner short: {}", short_lived_variable);

    // Эта переменная не видна внешней функции
    let long_lived_variable = 5_f32;

    println!("inner long: {}", long_lived_variable);
}
// Конец блока

// Ошибка! `short_lived_variable` не существует в этой области
println!("outer short: {}", short_lived_variable);
// ИСПРАВЬТЕ ^ Закомментируйте строку

println!("outer long: {}", long_lived_variable);
}

fn main() {
// Объявляем переменную
let a_variable;

{
    let x = 2i;

    // Инициализируем переменную
    a_variable = x * x;
}

println!("a variable: {}", a_variable);

let another_variable;

// Ошибка! Использование неинициализированной переменной
println!("another variable: {}", another_variable);
// ИСПРАВЬТЕ ^ Закомментируйте строку

another_variable = 1i;

println!("another variable: {}", another_variable);
}


fn main() {
// Аннотированный тип переменной
let a_float: f64 = 1.0;

// Эта переменная типа `int`
let mut an_integer = 5i;

// Ошибка! Тип переменной нельзя изменять
an_integer = true;
}

fn main() {
let decimal = 65.4321_f32;

// Ошибка! Нет неявного преобразования
let integer: u8 = decimal;
// ИСПРАВЬТЕ ^ Закомментируйте строку

// Явное преобразование
let integer = decimal as u8;
let character = integer as char;

println!("Casting: {} -> {} -> {}", decimal, integer, character);
}

fn main() {
// Литералы с суффиксами, их вид известен при инициализации
let x = 1u8;
let y = 2u;
let z = 3f32;

// Литералы без суффикса, их вид зависит от того, как они используются
let i = 1;
let f = 1.0;

// `size_of_val` возвращает размер переменной в байтах
println!("size of `x` in bytes: {}", std::mem::size_of_val(&x));
println!("size of `y` in bytes: {}", std::mem::size_of_val(&y));
println!("size of `z` in bytes: {}", std::mem::size_of_val(&z));
println!("size of `i` in bytes: {}", std::mem::size_of_val(&i));
println!("size of `f` in bytes: {}", std::mem::size_of_val(&f));

// Ограничения (слагаемые должны иметь тот же тип) для `i` и `f`
let _constraint_i = x + i;
let _constraint_f = z + f;
// Закомментируйте эти две строки
}

fn main() {
// Использование локального вывода, компилятор знает, что `elem` имеет тип `u8`
let elem = 5u8;

// Создадим пустой вектор (расширяемый массив)
let mut vec = Vec::new();
// В этот момент компилятор не знает точный тип `vec`, он
// просто знает, что это вектор `Vec<_>`

// Вставим `elem` в вектор
vec.push(elem);
// Ага! Теперь компилятор знает, что `vec` это вектор `u8` (`Vec<u8>`)
// Попробуйте закомментировать строку `vec.push(elem)`

println!("{}", vec);
}


// `NanoSecond` это новое имя для `u64`
type NanoSecond = u64;
type Inch = u64;

// Используйте этот атрибут, чтобы не выводить предупреждение
#[allow(non_camel_case_types)]
type uint64_t = u64;
// Попробуйте удалить атрибут

fn main() {
// `NanoSecond` = `Inch` = `uint64_t` = `u64`
let nanoseconds: NanoSecond = 5 as uint64_t;
let inches: Inch = 2 as uint64_t;

// Обратите внимание, что псевдонимы новых типов не предоставляют
// дополнительную безопасность, из-за того, что они не нового типа
println!("{} nanoseconds + {} inches = {} unit?",
nanoseconds,
inches,
nanoseconds + inches);
}
