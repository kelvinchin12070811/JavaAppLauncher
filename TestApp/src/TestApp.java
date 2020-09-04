import java.lang.management.ManagementFactory;
import java.lang.management.RuntimeMXBean;
import java.util.List;

public class TestApp
{
    public static void main(String[] args)
    {
        System.out.println("Starting application");
        System.out.println("Applications args:");
        for (String arg : args)
            System.out.printf("    %s\n", arg);

        RuntimeMXBean runtimeMXBean = ManagementFactory.getRuntimeMXBean();
        List<String> jvmArgs = runtimeMXBean.getInputArguments();

        System.out.println("\nJVM args:");
        for (String arg : jvmArgs)
            System.out.printf("    %s\n", arg);
        
        MainFrame frame = new MainFrame(args, jvmArgs);
        frame.display();
    }
}
