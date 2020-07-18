
public class TestApp
{
    public static void main(String[] args)
    {
        System.out.println("Starting application");
        System.out.println("Applications args:");
        for (String arg : args)
            System.out.printf("    %s\n", arg);
        
        MainFrame frame = new MainFrame(args);
        frame.display();
    }
}
