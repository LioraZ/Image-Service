   M S B A M L   `   `   `  ����I  EImageServieGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ImageServieGUI.ViewImageServieGUI.View  5      E'clr-namespace:ImageServieGUI.ConvertersImageServieGUI.Converters  5      "    ImageServieGUI.View.LogsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:ImageServieGUI.View     5      =	converter'clr-namespace:ImageServieGUI.Converters     5        � 	Resources  5
      8  2ImageServieGUI.Converters.MessageTypeEnumConverter   MessageTypeEnumConverter��%   &	          5   
   6-   5      .��5      �� .+�5   
   �� $��25��6   6"   �� 5   
   $
��Logs��6   $��Center=�6    $��138,0,122,0q�6=    J� RenderTransformOrigin$ 0.554,3.183&�6R   $��30��6v   $	��100��6�   $��20(�6�   &   System.Windows.Controls.DataGrid  5   
      
IsReadOnly .    AutoGenerateColumns .     CanUserAddRows .  "   VerticalScrollBarVisibility$
 Auto=�5   R   $��Center=�5      $��	15,33,5,0q�6*   $   HorizontalScrollBarVisibility$
 Auto=�6=   $	��Top=�5      ��6+   �� *Logs+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Status��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 65   �� *Status+
   	CellStyle
 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    .�5      ��  �� Property � �� �� Value 6;   �� *Status+ �� 	Converter# [  5      5        5      $ Message��6%   	 66   �� *	Message+5      5      5       