# NumberBox
Custom WPF TextBox controls for Decimals and Integers

## Usage
```
<controls:IntegerBox Value="{Binding Path=Age}" Maximum="100"/>
```

```
<controls:DecimalBox Value="{Binding Path=Percentage}" Maximum="98.5"/>
```

## Features
- Blocks invalid characters from being entered, only numbers and comma (DecimalBox) are allowed
- TwoWay-Binding by default
- Binding value is updated as Text changes
- Invalid input results into null value
- Max value can be defined
