# Quy định dành cho Inheritance:

Nếu 1 class MonoBehaviour có chủ ý cho phép kế thừa. Thì các phương thức của Unity đều phải khai báo `protected virtual`.
Tất cả các class kế thừa từ những class trên đều phải gọi base trước khi thực hiện thao tác khác.
Mục đích chính:
- Ngăn ngừa class con hide function Unity của class cha.
Test: 
- Scene `Assets/Examples/Inheritance/Inheritance.unity`
- Script `Assets/Examples/Inheritance/InheritanceManager.cs`
