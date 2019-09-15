import javax.swing.*;

class DemoApp
{
    public static void main(String[] args)
    {
        JOptionPane.showMessageDialog(null, "Hello World");
		String argline = String.join(",", args);
		JOptionPane.showMessageDialog(null, argline);
    }
}