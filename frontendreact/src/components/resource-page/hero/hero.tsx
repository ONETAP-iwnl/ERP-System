'use client'

import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css"
import { getResources } from '@/services/api/resourceClient';

interface Resource {
  id: number;
  name: string;
  isActive: boolean;
};

export default function Hero() {
    const router = useRouter();
    const [resource, setResource] = useState<Resource[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function fetchResource() {
            try {
                const data = await getResources();
                setResource(data);
            } catch(error) {
                console.error('Ошибка при загрузке ресурсов:', error);
            } finally {
                setLoading(false);
            }
        }
        fetchResource();
    }, []);

    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурсы</h1>
             <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={() => router.push('/resource/create')}>Добавить</button>
                <button className={styles.archiveButton} onClick={() => router.push('/resource/archive')}>К архиву</button>
            </div>
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>Наименование</th>
                    </tr>
                </thead>
                <tbody>
                    {resource.map(resources=> (
                        <tr key={resources.id} onClick={() => router.push(`/edit-resource/${resources.id}`)}>
                            <td>{resources.name}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </main>
    )
}